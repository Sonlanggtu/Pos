using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using static Pos.Customer.Common.CommonCustomers;
using Pos.Customer.Domain.CustomerAggregate;
namespace Pos.Customer.Infrastructure.Models
{
    public partial class POSCustomerContext : DbContext
    {
        public POSCustomerContext()
        {
        }

        public POSCustomerContext(DbContextOptions<POSCustomerContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public virtual DbSet<Customer.Domain.CustomerAggregate.Customer> Customer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var POSConnection = GetEnvConnection("CUSTOMER_READ_CONNECTION");
                optionsBuilder.UseSqlServer(POSConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Customer.Domain.CustomerAggregate.Customer>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });
        }
    }
}
