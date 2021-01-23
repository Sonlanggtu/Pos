using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerService;
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

        private POS_GatewaySecuritiesContext dbcontext = new POS_GatewaySecuritiesContext();
        private readonly CustomerServicce.CustomerServicceClient _customer;
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService, CustomerServicce.CustomerServicceClient customer)
        {
            _authService = authService;
            _customer = customer;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] Authentication auth)
        {
            if (string.IsNullOrEmpty(auth.Username) || string.IsNullOrEmpty(auth.Password))
                return BadRequest(new { message = "Tài khoản và mật khẩu là bắt buộc" });

            var listUser =  await dbcontext.AspNetUsers.AsNoTracking()
                                            .Where(x => x.UserName == auth.Username
                                                    && x.LockoutEnabled == false)
                                            .ToListAsync();

            listUser = listUser.AsEnumerable()
                               .Where(x=> PosEncryption.ValidatePassword(auth.Password, x.PasswordHash))
                               .ToList();
            var user = listUser.FirstOrDefault();

            // Test GRPC
            //string idUser = "858B5A7B-24A8-4C6C-BB7E-31F6A9E701CC";
            //var customer = new GetCustomerRequest { Id = idUser };
            //var result = await _customer.GetCustomerAsync(customer);
            // end GRPC

            if (user != null)
                return Ok(await _authService.GenerateTokenJWT(user));
            else
                return NotFound($"Không tìm thấy tài khoản {auth.Username}");
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
