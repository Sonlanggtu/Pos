﻿using AutoMapper;
using Dermayon.Common.CrossCutting;
using Dermayon.Common.Infrastructure.EventMessaging;
using Dermayon.Infrastructure.Data.EFRepositories.Contracts;
using Dermayon.Infrastructure.EvenMessaging.Kafka.Contracts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Pos.Customer.Domain.CustomerAggregate;
using Pos.Customer.Domain.Events;
using Pos.Customer.Infrastructure;
using Pos.Customer.Infrastructure.Models;
using Pos.Customer.WebApi.Application.Commands;
using Pos.Customer.WebApi.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pos.Customer.WebApi.Application.EventHandlers
{
    public class CustomerDeleteEventHandler : IServiceEventHandler
    {
        private readonly IUnitOfWork<POSCustomerContext> _uow;
        private readonly IMapper _mapper;
        private readonly IKakfaProducer _producer;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerQueries _customerQueries;

        public CustomerDeleteEventHandler(IUnitOfWork<POSCustomerContext> uow,
            IMapper mapper,
            IKakfaProducer producer,
            ICustomerRepository customerRepository,
            ICustomerQueries customerQueries)
        {
            _uow = uow;
            _mapper = mapper;
            _producer = producer;
            _customerQueries = customerQueries;
            _customerRepository = customerRepository;
        }
        public async Task Handle(JObject jObject, ILog log, CancellationToken cancellationToken)
        {
            try
            {
                log.Info("Consume CustomerDeletedEvent");

                var dataConsomed = jObject.ToObject<CustomerDeletedEvent>();

                var data = await _customerQueries.GetCustomer(dataConsomed.CustomerId);

                log.Info("Delete Customer");

                //Consume data to Read Db
                _customerRepository.Delete(data);
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                log.Error("Error Deleteing data customer", ex);
                throw ex;
            }            
        }
    }
}
