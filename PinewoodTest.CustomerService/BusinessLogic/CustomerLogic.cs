namespace PinewoodTest.CustomerService.BusinessLogic;
using Data;
using Dtos;

internal class CustomerLogic(
    CustomerRepository repository
) : ICustomerLogic
{
    private readonly CustomerRepository _repository = repository;

    public IEnumerable<CustomerDto> GetAll()
    {
        return _repository.GetAll();
    }

    public async Task<CustomerDto> GetById(Guid id)
    {
        var existingCustomer = await _repository.TryGetById(id);

        if (existingCustomer is null)
        {
            throw new InvalidDataException(
                $"No customer found with id {id}."
            );
        }

        return existingCustomer;
    }

    public async Task<CustomerDto> Add(CustomerDto newCustomer)
    {
        // For the sake of having something to do here, I'm going to ensure that name is unique
        // across customers.
        var existingCustomer = await _repository.TryGetByName(newCustomer.Name);

        if (existingCustomer is not null)
        {
            // For now I'm simply going to throw
            throw new InvalidDataException(
                $"A customer with this name already exists {newCustomer.Name}."
            );
        }

        newCustomer.SetId(Guid.NewGuid());

        return await _repository.Add(newCustomer);
    }

    public async Task<CustomerDto> Update(CustomerDto updatedCustomer)
    {
        if (updatedCustomer.Id is null)
        {
            // In theory, we could check by name for a match, but for now I'm going
            // to say that this is an invalid operation and simply throw.

            throw new InvalidDataException(
                $"An id must be passed for the customer you wish to update."
            );
        }

        var customerExists = await _repository.CustomerExistsById(updatedCustomer.Id.Value);
        if (!customerExists)
        {
            throw new InvalidDataException(
                $"No existing customer could be found to update with id {updatedCustomer.Id}."
            );
        }

        return await _repository.Update(updatedCustomer);
    }

    public async Task<bool> Delete(CustomerDto customerToDelete)
    {
        if (customerToDelete.Id is null)
        {
            // In theory, we could check by name for a match, but for now I'm going
            // to say that this is an invalid operation and simply throw.

            throw new InvalidDataException(
                $"An id must be passed for the customer you wish to delete."
            );
        }

        var customerExists = await _repository.CustomerExistsById(customerToDelete.Id.Value);
        if (!customerExists)
        {
            throw new InvalidDataException(
                $"No existing customer could be found to delete with id {customerToDelete.Id}."
            );
        }

        return await _repository.Delete(customerToDelete);
    }
}