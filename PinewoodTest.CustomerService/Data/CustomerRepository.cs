using Microsoft.EntityFrameworkCore;

namespace PinewoodTest.CustomerService.Data;
using Context;
using Dtos;
using Mappers;

internal class CustomerRepository(
    CustomerContext context,
    ILogger<CustomerRepository> logger
)
{
    private readonly CustomerContext _context = context;
    private readonly ILogger<CustomerRepository> _logger = logger;

    internal IEnumerable<CustomerDto> GetAll()
        => _context.Customers
            .Select(entity => entity.ToDto());

    internal async Task<CustomerDto?> TryGetById(Guid id)
    {
        var match = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(customer =>
                customer.Id == id
            );

        return match?.ToDto();
    }

    internal async Task<CustomerDto?> TryGetByName(string name)
    {
        var match = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(customer =>
                customer.Name == name
            );

        return match?.ToDto();
    }

    internal async Task<bool> CustomerExistsById(Guid id)
    {
        var exists = await _context.Customers
            .AsNoTracking()
            .AnyAsync(customer =>
                customer.Id == id
            );

        return exists;
    }

    internal async Task<CustomerDto> Add(CustomerDto newCustomer)
    {
        _logger.LogInformation(
            "New Customer added - Name {name}, IsActive {isActive}",
            newCustomer.Name,
            newCustomer.IsActive
        );

        var newCustomerEntity = newCustomer.ToEntity();
        _context.Customers
            .Add(newCustomerEntity);

        // Since we now reach a point where exceptions can happen outside of our control, this is where
        // I would use a try/catch. For now, we'll just log failure and throw.
        try
        {
            await _context.SaveChangesAsync();

            return newCustomerEntity.ToDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to save new customer with name {name} and IsActive {isActive}",
                newCustomer.Name,
                newCustomer.IsActive
            );

            throw new Exception(
                "Error occurred whilst saving new customer",
                ex
            );
        }
    }

    internal async Task<CustomerDto> Update(CustomerDto updatedCustomer)
    {
        _logger.LogInformation(
            "Customer updated - Id {id}, Name {name}, IsActive {isActive}",
            updatedCustomer.Id,
            updatedCustomer.Name,
            updatedCustomer.IsActive
        );

        var updatedCustomerEntity = updatedCustomer.ToEntity();
        _context.Customers
            .Update(updatedCustomerEntity);

        // Since we now reach a point where exceptions can happen outside of our control, this is where
        // I would use a try/catch. For now, we'll just log failure and throw.
        try
        {
            await _context.SaveChangesAsync();

            return updatedCustomerEntity.ToDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to save updated customer with id {id}, name {name} and IsActive {isActive}",
                updatedCustomer.Id,
                updatedCustomer.Name,
                updatedCustomer.IsActive
            );

            throw new Exception(
                "Error occurred whilst saving updated customer",
                ex
            );
        }
    }

    internal async Task<bool> Delete(CustomerDto customerToDelete)
    {
        _logger.LogInformation(
            "Customer deleted - Id {id}",
            customerToDelete.Id
        );

        _context.Customers
            .Remove(customerToDelete.ToEntity());

        // Since we now reach a point where exceptions can happen outside of our control, this is where
        // I would use a try/catch. For now, we'll just log failure and throw.
        try
        {
            var recordsUpdated = await _context.SaveChangesAsync();

            return recordsUpdated == 1;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to delete customer with id {id}",
                customerToDelete.Id
            );

            throw new Exception(
                "Error occurred whilst saving updated customer",
                ex
            );
        }
    }
}