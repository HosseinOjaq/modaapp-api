using System.IdentityModel.Tokens.Jwt;

namespace ModaApp.Application.Models.Services;

public record AccessTokenResponse(JwtSecurityToken SecurityToken, string RefreshToken)
{
    public string AccessToken { get; set; } = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
    public string RefreshToken { get; set; } = RefreshToken;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; } = (int)(SecurityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
}