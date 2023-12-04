using Microsoft.AspNetCore.Mvc;

namespace PinewoodTest.CustomerService.Api;
using BusinessLogic;
using Filters;
using Models;

[Route("api/[controller]")]
[ApiController]
public class CustomerController(
    ICustomerLogic logic
) : ControllerBase
{
    private readonly ICustomerLogic _logic = logic;

    [HttpGet]
    public IEnumerable<Customer> Get()
    {
        var allCustomers = _logic.GetAll();

        return allCustomers
            .Select(dto => new Customer(dto));
    }

    [HttpGet("/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [InvalidDataExceptionFilter]
    public async Task<ActionResult> GetById(Guid id)
    {
        var customerDto = await _logic.GetById(id);

        return Ok(new Customer(customerDto));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [InvalidDataExceptionFilter]
    public async Task<ActionResult> Add(Customer newCustomer)
    {
        var customerDto = newCustomer.ToDto();

        await _logic.Add(customerDto);

        return Created(
            $"{RootUrl}/{customerDto.Id}",
            new Customer(customerDto)
        );
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [InvalidDataExceptionFilter]
    public async Task<ActionResult> Update(Customer updatedCustomer)
    {
        var customerDto = updatedCustomer.ToDto();

        await _logic.Update(customerDto);

        return Accepted();
    }

    private string RootUrl
    {
        get
        {
            var request = HttpContext.Request;

            return $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}";
        }
    }
}