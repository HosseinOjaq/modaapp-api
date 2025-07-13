using ModaApp.Common.Models;
using ModaApp.Domain.Entities;
using ModaApp.Application.Models.Dtos;
using ModaApp.Application.Models.Services;

namespace ModaApp.Application.Common.Contracts;

public interface IAccountingService
{
    Task<OperationResult<LogOutTokenResponseDto>> BlockTokenAsync(CheckTokenRequestDto request);
    OperationResult<bool> IsTokenBlocked(CheckTokenRequestDto request);
    OperationResult<AccessTokenResponse> GenerateTokenAsync(User user);
    Task<OperationResult<AccessTokenResponse>> RefreshTokenAsync(string token, string refreshToken);
    Task<NotificationSendResult> SendActivationCodeToPhoneAsync(string phoneNumber);
    Task<NotificationSendResult> SendActivationCodeToEmailAsync(string email);
    List<ForgetPasswordOptionDto> GetForgetPasswordOptionsByUser(User user);
}