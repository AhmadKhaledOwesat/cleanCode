using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MobCentra.Application.Dto;
using MobCentra.Application.Interfaces;

namespace MobCentra.Application.Bll
{
    public class AuthenticationManager(IConfiguration configuration) : IAuthenticationManager
    {
        private const string TokenTypeAccess = "access";
        private const string TokenTypeRefresh = "refresh";
        private static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromMinutes(15);
        private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(7);

        public Tokens GenerateToken(string userName, object id)
        {
            var tokenKey = GetTokenKey();
            var handler = new JwtSecurityTokenHandler();
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);

            // Access token (short-lived)
            var accessClaims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.UserData, $"{id}"),
                new Claim("token_type", TokenTypeAccess)
            };
            var accessDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(accessClaims),
                Expires = DateTime.UtcNow.Add(AccessTokenLifetime),
                SigningCredentials = signingCredentials
            };
            var accessToken = handler.CreateToken(accessDescriptor);
            var accessTokenString = handler.WriteToken(accessToken);

            // Refresh token (long-lived)
            var refreshClaims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.UserData, $"{id}"),
                new Claim("token_type", TokenTypeRefresh)
            };
            var refreshDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(refreshClaims),
                Expires = DateTime.UtcNow.Add(RefreshTokenLifetime),
                SigningCredentials = signingCredentials
            };
            var refreshToken = handler.CreateToken(refreshDescriptor);
            var refreshTokenString = handler.WriteToken(refreshToken);

            return new Tokens { Token = accessTokenString, RefreshToken = refreshTokenString };
        }

        public Tokens RefreshToken(string bearerToken)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
                throw new UnauthorizedAccessException("Authorization header is required. Send your refresh token (the refreshToken from login), not the access token.");
            bearerToken = bearerToken.Replace("Bearer ", string.Empty, StringComparison.OrdinalIgnoreCase).Trim();
            var tokenKey = GetTokenKey();
            // Allow expired tokens for refresh: client can send either expired access token or refresh token
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };
            var handler = new JwtSecurityTokenHandler();
            System.Security.Claims.ClaimsPrincipal principal;
            try
            {
                principal = handler.ValidateToken(bearerToken, validationParameters, out _);
            }
            catch (Exception ex) when (ex is SecurityTokenInvalidSignatureException or SecurityTokenException)
            {
                throw new UnauthorizedAccessException("Invalid token. Signature or format is invalid.", ex);
            }
            // Accept both access and refresh token types so expired access token can be used to refresh
            var tokenType = principal.FindFirst("token_type")?.Value;
            if (tokenType != TokenTypeAccess && tokenType != TokenTypeRefresh)
                throw new UnauthorizedAccessException("Invalid token type.");
            var name = principal.FindFirst(ClaimTypes.Name)?.Value;
            var userData = principal.FindFirst(ClaimTypes.UserData)?.Value;
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(userData))
                throw new UnauthorizedAccessException("Invalid token claims.");
            return GenerateToken(name, userData);
        }

        private byte[] GetTokenKey() =>
            Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? configuration["JWT:Key"] ?? throw new InvalidOperationException("Jwt:Key is required"));
    }
}
