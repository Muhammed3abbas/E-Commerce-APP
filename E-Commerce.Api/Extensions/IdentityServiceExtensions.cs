using E_Commerce.Api.Errors;
using E_Commerce.BLL.Interfaces;
using E_Commerce.DAL.Entities.Identity;
using E_Commerce.DAL.Identity;
using E_Commerce.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace E_Commerce.Api.Extensions
{
    public static class IdentityServiceExtensions
    {

        public static IServiceCollection AddIdentityServices(this IServiceCollection Services, IConfiguration Configuration)
        {


            // Add AppUser DbContext
            Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            });

            Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //options.Password.RequireUppercase = true;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();
            Services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme*/options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //Configuration Auth handler
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JWT:ValidationIssuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes((Configuration["JWT:SecretKey"]))),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromDays(double.Parse(Configuration["JWT:DurationInDays"])),
                };



            });
            Services.AddScoped(typeof(IAuthService), typeof(AuthService));

            Services.AddAuthorization();

            return Services;

        }
    }
}
