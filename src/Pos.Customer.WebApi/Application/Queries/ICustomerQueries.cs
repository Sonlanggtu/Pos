//using Pos.Customer.Domain.CustomerAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pos.Customer.Domain.CustomerAggregate;
namespace Pos.Customer.WebApi.Application.Queries
{
    public interface ICustomerQueries
    {
        Task<Domain.CustomerAggregate.Customer> GetCustomer(Guid id);
        Task<IEnumerable<Domain.CustomerAggregate.Customer>> GetCustomers();
        Task<IEnumerable<Domain.CustomerAggregate.Customer>> GetCustomer(string name);
    }
}
