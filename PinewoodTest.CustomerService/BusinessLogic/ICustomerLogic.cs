namespace PinewoodTest.CustomerService.BusinessLogic;
using Dtos;

public interface ICustomerLogic
{
    IEnumerable<CustomerDto> GetAll();
    Task<CustomerDto> GetById(Guid id);
    Task<CustomerDto> Add(CustomerDto newCustomer);
    Task<CustomerDto> Update(CustomerDto updatedCustomer);
    Task<bool> Delete(CustomerDto customerToDelete);
}