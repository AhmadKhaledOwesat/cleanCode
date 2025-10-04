﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using DcpTracker.Application.Interfaces;

namespace DcpTracker.Application.Bll
{
    public class IdentityManager<TId>(IHttpContextAccessor _httpContextAccessor, IConfiguration _configuration) : IIdentityManager<TId> where TId : struct
    {
        public TId CurrentUserId { get { return GetCurrentUserId(); } }
        private TId GetCurrentUserId()
        {
            string token = _httpContextAccessor!.HttpContext!.Request.Headers["Authorization"]!.FirstOrDefault()?.Replace("Bearer ", string.Empty) ?? string.Empty;
            JwtSecurityTokenHandler handler = new();
            TokenValidationParameters validations = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            handler.ValidateToken(token, validations, out var validatedToken);
            IEnumerable<Claim> claims = ((JwtSecurityToken)validatedToken).Claims;
            var claimValue = claims.SingleOrDefault(a => a.Type == ClaimTypes.UserData)?.Value;
            if (typeof(TId) == typeof(Guid))
            {
                return (TId)(object)Guid.Parse(claimValue!);
            }
            else if (typeof(TId) == typeof(int))
            {
                return (TId)(object)int.Parse(claimValue!);
            }
            else if (typeof(TId) == typeof(string))
            {
                return (TId)(object)claimValue!;
            }
            else
            {
                throw new NotSupportedException($"TId type '{typeof(TId)}' is not supported.");
            }
        }
    }
}
