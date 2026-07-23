using E_Commerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.Infrastructure.Identity.Services
{
    internal class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtOptions;

        public TokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        public string CreateToken(string userId, string email, string userName, IReadOnlyList<string> roles)
        {
            //Claims
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, userName)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


            //signing credentials [Secret Key, Security Algorithm]
            var secKey = _jwtOptions.SecretKey;
            if (string.IsNullOrWhiteSpace(secKey))
              throw new InvalidOperationException("JWT Secrect Key Is Missing");

            if (secKey.Length < 32)
                throw new InvalidOperationException("JWT Secrect Key Is Too Short");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
                signingCredentials: credentials 
            );
                
          return  new JwtSecurityTokenHandler().WriteToken(token);

        }

        
    }
    public class JwtSettings
    {
        public string SecretKey { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int ExpirationInMinutes { get; set; }
    }
}
