using CustomerService;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos.Customer.WebApi.Services
{
    public class CustomerService : CustomerServicce.CustomerServicceBase
    {
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
        }

        public GetCustomerReply GetCustomer(GetCustomerRequest request, ServerCallContext context)
        {
            var transactionId = Guid.NewGuid().ToString();
            _logger.LogInformation($"transactionId : {transactionId}");


            //await _shippings.SendOrderAsync(new SendOrderRequest
            //{
            //    ProductId = request.ProductId,
            //    Quantity = request.Quantity,
            //    Address = request.Address,
            //    OrderId = new Guid("A3CDAD9BF7FA4699AE38CB68278089FB").ToString()
            //});

            return (new GetCustomerReply
            {
                Name = "Name1",
                Address = "adress13",
                Phone = "099392432432",
                Id = transactionId
            });
        }
    }
}
