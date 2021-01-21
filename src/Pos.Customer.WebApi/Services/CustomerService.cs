using CustomerService;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Pos.Customer.Infrastructure;
using Pos.Customer.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginService;
namespace Pos.Customer.WebApi.Services
{
    public class CustomerService : CustomerServicce.CustomerServicceBase
    {
        private POSCustomerContext dbContext = new POSCustomerContext();
        private readonly ILogger<CustomerService> _logger;


        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
        }

        public async override Task<GetCustomerReply> GetCustomer(GetCustomerRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"customer input value >>>> {request.Id}");
            var idCustomer = Guid.Parse(request.Id);
            var customer = await dbContext.Customer.FindAsync(idCustomer);
            return (new GetCustomerReply
            {
                Name = customer.Name,
                Address = customer.Address,
                Phone = customer.Phone,
                Id = customer.Id.ToString(),
                CreatedBy = customer.CreatedBy,
                CreatedDate = customer.CreatedDate.ToString(),
                ModifiedBy = customer.ModifiedBy,
                ModifiedDate = customer.ModifiedDate.ToString()
            });
        }
    }
}
