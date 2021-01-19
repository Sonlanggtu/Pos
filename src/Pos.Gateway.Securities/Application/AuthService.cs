using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Pos.Gateway.Securities.Common;
using Pos.Gateway.Securities.Models;
using Pos.Gateway.Securities.ViewModels;

namespace Pos.Gateway.Securities.Application
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        //private readonly UserManager<AspNetUsers> _userManager;
        private static POS_GatewaySecuritiesContext dbContext = new POS_GatewaySecuritiesContext();
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // authentication successful so generate jwt token
        public async Task<object> GenerateTokenJWT(AspNetUsers user)
        {
           // var roles = await _userManager.GetRolesAsync(user);
           // var permissions = await GetPermissionByUserId(user.Id.ToString());
            var claims = new[]
            {
                    new Claim("Email", user.Email),
                    new Claim(GatewaySecureCommon.UserClaim.Id, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(GatewaySecureCommon.UserClaim.FullName, user.FullName??string.Empty),
                    // new Claim(GatewaySecureCommon.UserClaim.Roles, string.Join(";", roles)),
                    // new Claim(GatewaySecureCommon.UserClaim.Permissions, JsonConvert.SerializeObject(permissions)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                _configuration["Tokens:Issuer"],
                 claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: creds);
            return (new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        public async Task<object> CreateAsync(AspNetUserVm user)
        {
            try
            {
                var createUser = new AspNetUsers();
                createUser.Id = Guid.NewGuid().ToString();
                createUser.UserName = user.UserName;
                createUser.FullName = user.FullName;
                //createUser.NormalizedUserName = user.NormalizedUserName;
                createUser.Email = user.Email;
                //createUser.NormalizedEmail = user.NormalizedEmail;
                createUser.EmailConfirmed = user.EmailConfirmed;
                createUser.PasswordHash = PosEncryption.HashPassword(user.PasswordHash);
                createUser.PhoneNumber = user.PhoneNumber;
                dbContext.AspNetUsers.Add(createUser);
                await dbContext.SaveChangesAsync();
                return new { status = GatewaySecureCommon.Success, reuslt = createUser };
            }
            catch (Exception ex)
            {

                return new { status = GatewaySecureCommon.ErrorSystem, reuslt = "" };
            }





        }
    }
}
