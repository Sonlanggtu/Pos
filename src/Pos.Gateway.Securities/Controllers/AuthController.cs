using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pos.Gateway.Securities.Application;
using Pos.Gateway.Securities.Common;
using Pos.Gateway.Securities.Models;
using Pos.Gateway.Securities.ViewModels;

namespace Pos.Gateway.Securities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        //private readonly SignInManager<AspNetUsers> _signInManager;
        //private readonly UserManager<AspNetUsers> _userManager;
        private POS_GatewaySecuritiesContext dbcontext = new POS_GatewaySecuritiesContext();
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] Authentication auth)
        {
            if (string.IsNullOrEmpty(auth.Username) || string.IsNullOrEmpty(auth.Password))
                return BadRequest(new { message = "Tài khoản và mật khẩu là bắt buộc" });

            var user = await dbcontext.AspNetUsers.Where(x=> x.UserName == auth.Username
                                                            && PosEncryption.ValidatePassword(x.PasswordHash, auth.Password)
                                                            && x.LockoutEnabled == false)
                                                            .FirstOrDefaultAsync();
            if (user != null)
            {
                //var result = await _signInManager.PasswordSignInAsync(auth.Username, auth.Password, false, true);
                //if (!user.Succeeded)
                //    return BadRequest("Mật khẩu không đúng");
                return Ok(await _authService.GenerateTokenJWT(user));
            }
            else
            {
                return NotFound($"Không tìm thấy tài khoản {auth.Username}");
            }
        }


        [AllowAnonymous]
        [HttpPost("RegisterAsync")]
        public async Task<IActionResult> RegisterAsync([FromBody] AspNetUserVm user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.PasswordHash))
                return BadRequest(new { message = "Tài khoản và mật khẩu là bắt buộc" });

            var infoUsers = await _authService.CreateAsync(user);
            return Ok(infoUsers);
        }
    }
}
