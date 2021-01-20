using LoginService;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerService;

namespace Pos.Gateway.Securities.Services
{
    public class LoginService : LoginServicce.LoginServicceBase
    {
        private readonly ILogger<LoginService> _logger;
        private readonly CustomerServicce.CustomerServicceClient _customer;

        public LoginService(ILogger<LoginService> logger, CustomerServicce.CustomerServicceClient customer)
        {
            _logger = logger;
            _customer = customer;
        }

        public override async Task<LoginReply> LoginSystem(LoginRequest request, ServerCallContext context)
        {

            string idUser = "858B5A7B-24A8-4C6C-BB7E-31F6A9E701CC";
            var customer = new GetCustomerRequest {
                Id = idUser
            };
            var result = await _customer.GetCustomerAsync(customer);

            return (new LoginReply
            {
              Id = Guid.NewGuid().ToString(),
              UserName = "NguyenA",
              FullName = "NguyenVanA",
              PhoneNumber = "912312321",
              Email = "NguyenVanA@gmail.com",
              LockoutEnabled = false
            });
        }
    }
}
