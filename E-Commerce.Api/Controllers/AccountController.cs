using AutoMapper;
using E_Commerce.Api.Dtos;
using E_Commerce.Api.Errors;
using E_Commerce.BLL.Interfaces;
using E_Commerce.DAL.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        //private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            //ITokenService tokenService,
            IMapper mapper, IAuthService authService)
        {
            _mapper = mapper;
            _authService = authService;
            //_tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager),
                DisplayName = user.DisplayName,

            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            var user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split("@")[0],
                PhoneNumber = registerDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = await _authService.CreateTokenAsync(user, _userManager),
                Email = user.Email
            };


        }
    }
}
