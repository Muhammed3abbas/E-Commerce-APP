using E_Commerce.DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<string> CreateTokenAsync(AppUser User,UserManager<AppUser> userManager);
    }
}
