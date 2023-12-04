using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PinewoodTest.CustomerService.Filters;

public class InvalidDataExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is InvalidDataException)
        {
            context.Result = new BadRequestResult();
        }
    }
}