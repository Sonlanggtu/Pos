using Dermayon.Infrastructure.Data.EFRepositories;
using Pos.Customer.Domain.CustomerAggregate;
using Pos.Customer.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pos.Customer.Infrastructure.Repositories
{
    public class CustomeRepository : EfRepository<Domain.CustomerAggregate.Customer>, ICustomerRepository
    {
        private readonly POSCustomerContext _context;
        public CustomeRepository(POSCustomerContext context) : base(context)
        {
            _context = context;
        }
    }
}
