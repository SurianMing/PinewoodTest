using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PinewoodTest.CustomerService.Data.Context.EntityModels;

public class Customer
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public bool IsActive { get; set; }
}

internal class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer");

        builder.HasKey(model => model.Id);

        builder.Property(model => model.Name)
            .HasMaxLength(100);

        builder.HasIndex(model => model.Name);
    }
}