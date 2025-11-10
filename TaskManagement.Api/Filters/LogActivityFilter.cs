using System.Text.Json;

using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskManagement.Api.Filters;

public class LogActivityFilter : IAsyncActionFilter
{
    /*
    action filters are more deep than middlewares

     */

    private readonly ILogger<LogActivityFilter> _logger;

    public LogActivityFilter(ILogger<LogActivityFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation($"(info)--> Executing action {context.ActionDescriptor.DisplayName} on controller {context.Controller} with arguments {JsonSerializer.Serialize(context.ActionArguments)}.");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation($"(info)--> Action {context.ActionDescriptor.DisplayName} executed on controller {context.Controller}");
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogInformation($"(async)--> Executing action {context.ActionDescriptor.DisplayName} on controller {context.Controller} with arguments {JsonSerializer.Serialize(context.ActionArguments)}.");
        await next();
        _logger.LogInformation($"(async)--> Action {context.ActionDescriptor.DisplayName} executed on controller {context.Controller}");
    }
}
