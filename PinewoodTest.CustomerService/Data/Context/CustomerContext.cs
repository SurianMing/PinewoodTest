using Microsoft.EntityFrameworkCore;

namespace PinewoodTest.CustomerService.Data.Context;
using EntityModels;

public class CustomerContext(
    DbContextOptions<CustomerContext> options
) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
    }
}