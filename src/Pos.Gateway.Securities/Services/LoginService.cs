using LoginService;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerService;
using Pos.Gateway.Securities.Models;
using Microsoft.EntityFrameworkCore;
using Pos.Gateway.Securities.Common;

namespace Pos.Gateway.Securities.Services
{
    public class LoginService : LoginServicce.LoginServicceBase
    {
        private POS_GatewaySecuritiesContext dbContext = new POS_GatewaySecuritiesContext();
        private readonly ILogger<LoginService> _logger;
        private readonly CustomerServicce.CustomerServicceClient _customer;

        public LoginService(ILogger<LoginService> logger, CustomerServicce.CustomerServicceClient customer)
        {
            _logger = logger;
            _customer = customer;
        }

        public override async Task<LoginReply> LoginSystem(LoginRequest request, ServerCallContext context)
        {
            string status = GatewaySecureCommon.NotFound;
            try
            {
                var listUser = await dbContext.AspNetUsers.AsNoTracking()
                                       .Where(x => x.UserName == request.UserName
                                               && x.LockoutEnabled == false)
                                       .ToListAsync();

                listUser = listUser.AsEnumerable()
                                   .Where(x => PosEncryption.ValidatePassword(request.Password, x.PasswordHash))
                                   .ToList();
                var user = listUser.FirstOrDefault();
                if (user != null) status = GatewaySecureCommon.Success;
                return (new LoginReply
                {
                    Id = user?.Id ?? "",
                    UserName = user?.UserName ?? "",
                    FullName = user?.ToString() ?? "",
                    PhoneNumber = user?.PhoneNumber ?? "",
                    Email = user?.Email ?? "",
                    LockoutEnabled = user?.LockoutEnabled ?? true,
                    Status = status
                });
            }
            catch (Exception)
            {
                status = GatewaySecureCommon.ErrorSystem;
                return (new LoginReply
                {
                    Id = "",
                    UserName = "",
                    FullName = "",
                    PhoneNumber = "",
                    Email = "",
                    LockoutEnabled = true,
                    Status = status
                });
            }


        }
    }
}
