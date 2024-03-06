using HalloDoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Service.Interface
{
    public interface IJwtToken
    {
        string GenrateJwtToken(Aspnetuser aspnetuser);

        bool ValidateToken(string token, JwtSecurityToken jwtSecurityToken);
    }
}
