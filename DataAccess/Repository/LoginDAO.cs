using BusinessObject.DataAccess;
using BusinessObject;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataAccess.Repository
{
   
        public class LoginDAO : ILoginRepository
        {
           
            private readonly IConfiguration _iconfiguration;

            public LoginDAO( IConfiguration iconfiguration)
            {
              
                _iconfiguration = iconfiguration;
            }

     


            public Tokens Authenticate(Users users)
            {
            var context = new PRN231FamilyTreeContext();
            var role = "";
                var user = context.Accounts.FirstOrDefault(x => x.Email == users.Name && x.Password == users.Password);
                if (user == null)
                {
                    return null;
                }
                if(user.Role == 1)
            {
                role = "Admin";
            }else if (user.Role == 2) {
                role = "Customer";
            }
                // Generate JSON Web Token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_iconfiguration["JWT:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Email, users.Name),
                 new Claim(ClaimTypes.Role, role) 
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return new Tokens { Token = tokenHandler.WriteToken(token) };
            }
        

    }
}
