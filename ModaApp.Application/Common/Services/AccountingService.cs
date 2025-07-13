using System.Text;
using ModaApp.Domain.Enums;
using ModaApp.Common.Models;
using System.Security.Claims;
using ModaApp.Domain.Entities;
using ModaApp.Domain.Repositories;
using ModaApp.Domain.Caching.Enums;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ModaApp.Application.Models.Dtos;
using ModaApp.Domain.Configuration.SMS;
using ModaApp.Application.Models.Services;
using ModaApp.Domain.Caching.Abstractions;
using ModaApp.Application.Common.Contracts;

namespace ModaApp.Application.Common.Services;

public class AccountingService
    : IAccountingService
{
    private readonly ICache cache;
    private readonly IUnitOfWork uow;
    private readonly ISmsService smsService;
    private readonly IEmailService emailService;
    private readonly IUserRepository userRepository;
    private readonly IOptions<SiteSettings> siteSettings;
    private readonly IRefreshTokenRepository refreshTokenRepository;
    private readonly IOptions<TokenValidationParameters> tokenValidationParameters;
    private readonly IPhoneNumberTokenProviderConfiguration phoneNumberTokenProviderConfiguration;

    public AccountingService(
        ICache cache,
        IUnitOfWork uow,
        ISmsService smsService,
        IEmailService emailService,
        IUserRepository userRepository,
        IOptions<SiteSettings> siteSettings,
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<TokenValidationParameters> tokenValidationParameters,
        IPhoneNumberTokenProviderConfiguration phoneNumberTokenProviderConfiguration)
    {

        this.uow = uow;
        this.cache = cache;
        this.smsService = smsService;
        this.emailService = emailService;
        this.siteSettings = siteSettings;
        this.userRepository = userRepository;
        this.refreshTokenRepository = refreshTokenRepository;
        this.tokenValidationParameters = tokenValidationParameters;
        this.phoneNumberTokenProviderConfiguration = phoneNumberTokenProviderConfiguration;
    }

    public async Task<OperationResult<LogOutTokenResponseDto>> BlockTokenAsync(CheckTokenRequestDto request)
    {
        var principal = GetPrincipalFromTokenWithoutAlgorithmValidation(request.Token);
        if (principal is null)
            return new LogOutTokenResponseDto(true);

        var expiredDateString = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value;
        var expiredDuration = long.Parse(expiredDateString);
        var expiredDate = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(expiredDuration).ToLocalTime();
        if (expiredDate < DateTime.UtcNow)
            return new LogOutTokenResponseDto(true);

        var userId = principal.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
        var jwtId = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        var cacheKey = GetBlockedTokenCacheKey(userId, jwtId);
        var cachedToken = cache.Get<string>(cacheKey, CacheInstanceType.UserTokens);
        if (string.IsNullOrEmpty(cachedToken))
            cache.Set(cacheKey, request.Token, expiredDate, CacheInstanceType.UserTokens);

        var storedToken = await refreshTokenRepository.GetByToken(jwtId);
        if (storedToken is null)
            return new LogOutTokenResponseDto(true);

        storedToken.Invalidate();
        await uow.SaveChangesAsync();
        return new LogOutTokenResponseDto(true);
    }

    public OperationResult<bool> IsTokenBlocked(CheckTokenRequestDto request)
    {
        var principal = GetPrincipalFromTokenWithoutAlgorithmValidation(request.Token);
        if (principal is null)
            return true;

        var userId = principal.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
        var jwtId = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        var cacheKey = GetBlockedTokenCacheKey(userId, jwtId);
        var tokenCache = cache.Get<string>(cacheKey, CacheInstanceType.UserTokens);
        if (string.IsNullOrEmpty(tokenCache))
            return false;

        return true;
    }

    public OperationResult<AccessTokenResponse> GenerateTokenAsync(User user)
    {
        var secretKey = Encoding.UTF8.GetBytes(siteSettings.Value.JwtSettings.SecretKey); // longer that 16 character
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);
        var encryptionkey = Encoding.UTF8.GetBytes(siteSettings.Value.JwtSettings.EncryptKey); //must be 16 character
        var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

        var claims = GenerateClaims(user);
        var refreshToken = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!.Value;
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = siteSettings.Value.JwtSettings.Issuer,
            Audience = siteSettings.Value.JwtSettings.Audience,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow.AddMinutes(siteSettings.Value.JwtSettings.NotBeforeMinutes),
            Expires = DateTime.UtcNow.AddMinutes(siteSettings.Value.JwtSettings.ExpirationMinutes),
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptingCredentials,
            Subject = new ClaimsIdentity(claims)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);
        return new AccessTokenResponse(securityToken, refreshToken);
    }

    public async Task<OperationResult<AccessTokenResponse>> RefreshTokenAsync(string token, string refreshToken)
    {
        var validateToken = GetPrincipalFromToken(token);
        if (validateToken is null)
            return ErrorModel.Create("TokenIsInvalid", "توکن نامعتبر می باشد");

        var expiredDate = validateToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value;
        var expiredDateValue = long.Parse(expiredDate);
        var expiredDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiredDateValue);

        if (expiredDateUtc > DateTime.UtcNow)
            return ErrorModel.Create("TokenIsExpired", "توکن منقضی شده است");

        var storedToken = await refreshTokenRepository.GetByToken(refreshToken);
        if (storedToken is null)
            return ErrorModel.Create("TokenIsInvalid", "توکن نامعتبر می باشد");

        if (storedToken!.ExpiredDateOnUtc < DateTime.UtcNow)
            return ErrorModel.Create("TokenIsExpired", "توکن منقضی شده است");

        var jti = validateToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        if (storedToken.JwtId != jti)
            return ErrorModel.Create("TokenIsInvalid", "توکن نامعتبر می باشد");

        storedToken.UsedToken();
        var saveChangesResult = await uow.SaveChangesAsync();
        if (!saveChangesResult.IsSuccess)
            OperationResult<AccessTokenResponse>.Fail();

        var userId = Convert.ToInt32(validateToken.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        var user = await userRepository.FindAsync(userId);
        if (user is null)
            return ErrorModel.Create("UserNotFound", "کاربر یافت نشد");

        return GenerateTokenAsync(user);
    }

    public async Task<NotificationSendResult> SendActivationCodeToPhoneAsync(string phoneNumber)
    {
        var smsSendResult = new NotificationSendResult();
        var cacheKey = $"{phoneNumber}-sent-sms";
        if (cache.Exists(cacheKey, CacheInstanceType.Default))
        {
            smsSendResult.AlreadySend = true;
            smsSendResult.SuccessSent = true;

            var cacheTime = cache.TimeToLive(cacheKey, CacheInstanceType.Default);
            var cacheTimeTotalSeconds = Math.Round(cacheTime!.Value.TotalSeconds);
            var message = "پیامک از قبل برایتان ارسال گردیده است، لطفا TotalSeconds ثانیه صبر نمایید";
            message = message.Replace($"{nameof(cacheTime.Value.TotalSeconds)}", cacheTimeTotalSeconds.ToString());
            smsSendResult.Message = ErrorModel.Create("TheSmsHasAleardyBeenSentToYou", message);
        }
        else
        {
            var message = await smsService.GenerateTokenAsync(phoneNumber);
            var sendingSmsStatus = smsService.Send(phoneNumber, message);
            if (sendingSmsStatus)
                await cache.SetAsync(cacheKey, "", new TimeSpan(0, 0, 0, phoneNumberTokenProviderConfiguration.Duration), CacheInstanceType.Default);
            smsSendResult.SuccessSent = sendingSmsStatus;
            smsSendResult.Message = sendingSmsStatus
                                    ? ErrorModel.Create("VerificationCodeSentSuccessfully", "کد تایید با موفقیت ارسال شد")
                                    : ErrorModel.Create("VerificationCodeCouldNotBeSendSuccessfully", "کد تایید با موفقیت ارسال نشد");
        }
        return smsSendResult;
    }

    public async Task<NotificationSendResult> SendActivationCodeToEmailAsync(string email)
    {
        var smsSendResult = new NotificationSendResult();
        var cacheKey = $"{email}-sent-email";
        if (cache.Exists(cacheKey, CacheInstanceType.Default))
        {
            smsSendResult.AlreadySend = true;
            smsSendResult.SuccessSent = true;

            var cacheTime = cache.TimeToLive(cacheKey, CacheInstanceType.Default);
            var cacheTimeTotalSeconds = Math.Round(cacheTime!.Value.TotalSeconds);
            var message = "ایمیل از قبل برایتان ارسال گردیده است، لطفا TotalSeconds ثانیه صبر نمایید";
            message = message.Replace($"{nameof(cacheTime.Value.TotalSeconds)}", cacheTimeTotalSeconds.ToString());
            smsSendResult.Message = ErrorModel.Create("TheEmailHasAleardyBeenSentToYou", message);
        }
        else
        {
            var message = await emailService.GenerateTokenAsync(email);
            var sendingEmailStatus = emailService.Send(email, message, "کد تایید  - سازمان چاپ");
            if (sendingEmailStatus)
                await cache.SetAsync(cacheKey, "", new TimeSpan(0, 0, 0, phoneNumberTokenProviderConfiguration.Duration), CacheInstanceType.Default);
            smsSendResult.SuccessSent = sendingEmailStatus;
            smsSendResult.Message = sendingEmailStatus
                                    ? ErrorModel.Create("VerificationCodeSentSuccessfully", "کد تایید با موفقیت ارسال شد")
                                    : ErrorModel.Create("VerificationCodeCouldNotBeSendSuccessfully", "کد تایید با موفقیت ارسال نشد");
        }
        return smsSendResult;
    }

    public List<ForgetPasswordOptionDto> GetForgetPasswordOptionsByUser(User user)
    {
        var optionItems = new List<ForgetPasswordOptionDto>();
        if (user.PhoneNumberConfirmed && !string.IsNullOrEmpty(user.PhoneNumber))
            optionItems.Add(new ForgetPasswordOptionDto
            (
                $"{user.PhoneNumber[..4]}******{user.PhoneNumber[^2..]}",
                ForgetPasswordOptionType.Message
            ));
        if (user.EmailConfirmed && !string.IsNullOrEmpty(user.Email))
            optionItems.Add(new ForgetPasswordOptionDto
            (
                $"{user.Email[..2]}******{user.Email[user.Email.IndexOf('@')..]}",
                ForgetPasswordOptionType.Email
            ));
        return optionItems;
    }

    private static List<Claim> GenerateClaims(User user)
    {
        var jwtId = Guid.NewGuid().ToString();
        var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, jwtId),
            new(securityStampClaimType, user.SecurityStamp)
        };
        return claims;
    }

    private static bool IsJwtValidSecurityAlgorithm(SecurityToken securityToken)
    {
        try
        {
            return securityToken is JwtSecurityToken jwtSecurityToken &&
               jwtSecurityToken
               .Header.Alg
               .Equals(SecurityAlgorithms.HmacSha256,
               StringComparison.InvariantCultureIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    private ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenValidationParameters.Value.ValidateLifetime = false;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters.Value, out var validationToken);
            if (!IsJwtValidSecurityAlgorithm(validationToken))
                return null;
            return principal;
        }
        catch
        {
            return null;
        }
        finally
        {
            tokenValidationParameters.Value.ValidateLifetime = true;
        }
    }

    private ClaimsPrincipal? GetPrincipalFromTokenWithoutAlgorithmValidation(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenValidationParameters.Value.ValidateLifetime = false;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters.Value, out var validationToken);
            return principal;
        }
        catch
        {
            return null;
        }
        finally
        {
            tokenValidationParameters.Value.ValidateLifetime = true;
        }
    }

    private static string GetBlockedTokenCacheKey(string userId, string jwtId)
        => $"BlockedToken|{userId}|{jwtId}";
}