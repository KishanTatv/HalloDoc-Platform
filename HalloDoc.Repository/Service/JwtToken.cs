using HalloDoc.Entity.Models;
using HalloDoc.Repository.Service.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Service
{
    public class JwtToken : IJwtToken
    {
        private readonly IConfiguration _Iconfig;

        public JwtToken(IConfiguration config)
        {
            _Iconfig = config;
        }

        public string GenrateJwtToken(Aspnetuser aspnetuser)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Iconfig["Jwt:Key"]));
            var credintal = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, aspnetuser.Email),
                new Claim(ClaimTypes.Role, aspnetuser.Aspnetuserrole.Role.Name)
            };

            var token = new JwtSecurityToken(
                _Iconfig["Jwt:Issuer"],
                _Iconfig["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: credintal
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public bool ValidateToken(string token,out JwtSecurityToken jwtSecurityToken)
        {
            jwtSecurityToken = null;

            if (token == null)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _Iconfig["Jwt:Issuer"],
                    ValidAudience = _Iconfig["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Iconfig["Jwt:Key"]))
                }, out SecurityToken validatedToken);

                jwtSecurityToken = (JwtSecurityToken)validatedToken;
                if(jwtSecurityToken != null)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
