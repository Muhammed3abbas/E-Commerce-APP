using E_Commerce.BLL.Interfaces;
using E_Commerce.DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<string> CreateTokenAsync(AppUser User, UserManager<AppUser> userManager)
        {
            // Private Claim (User - Defined)
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,User.UserName),
                //new Claim("Name",User.UserName)
                new Claim(ClaimTypes.Email,User.Email)

            };

            var Roles = await userManager.GetRolesAsync(User);
            foreach (var role in Roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Security Key
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes((_config["JWT:SecretKey"])));

            var Token = new JwtSecurityToken(
                audience: _config["JWT:ValidAudience"],
                issuer: _config["JWT:ValidationIssuer"],
                expires: DateTime.Now.AddDays(double.Parse(_config["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials:new SigningCredentials(authKey,SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(Token); 
        }
    }
}
