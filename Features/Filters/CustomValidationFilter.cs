using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Features.Filters;

/// <summary>
/// Validation Filter
/// </summary>
public class CustomValidationFilter : IActionFilter
{
    /// <summary>
    /// OnActionExecuting
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errorMessage = context.ModelState
                .Where(ms => ms.Key == "Password" && ms.Value.Errors.Count > 0)
                .Select(ms => ms.Value.Errors.First().ErrorMessage)
                .FirstOrDefault();

            var errorResponse = new
            {
                Code = "INPUT_ERROR" ,
                Result = - 99,
                Message = errorMessage
            };

            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = 200
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}