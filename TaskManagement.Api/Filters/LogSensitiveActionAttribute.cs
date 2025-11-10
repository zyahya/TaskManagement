using System.Diagnostics;

using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskManagement.Api.Filters;

public class LogSensitiveActionAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        Debug.WriteLine("Sensitive action executed!!!!!!!!");
    }
}
