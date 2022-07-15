using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using RedditMockup.Common.Dtos;
using RedditMockup.Common.Helpers;
using RedditMockup.Common.ViewModels;

namespace RedditMockup.Api.Filters;

public class CustomExceptionFilter : ExceptionFilterAttribute
{
    #region [Field(s)]

    private readonly Logger _logger;

    #endregion

    #region [Constructor]

    public CustomExceptionFilter(IEnumerable<Logger> loggers) =>
        _logger = loggers.SingleOrDefault(x => x.Name == "Error")
              ?? throw new ArgumentNullException(nameof(Logger), @"Logger With Name 'Error' Not Registered !");

    #endregion

    #region [Override Method(s)]

    public override void OnException(ExceptionContext filterContext)
    {
        if (filterContext.ExceptionHandled) return;

        var mongoLog = new MongoLog
        {
            ActionName = filterContext.ActionDescriptor.RouteValues["action"],
            ControllerName = filterContext.ActionDescriptor.RouteValues["controller"],
            ClientIp = filterContext.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString(),
            ServerIp = filterContext.HttpContext.Features.Get<IHttpConnectionFeature>()?.LocalIpAddress?.MapToIPv4().ToString(),
            Exception = filterContext.Exception
        };

        if (filterContext.HttpContext.User.Identity?.IsAuthenticated ?? false)
            mongoLog.Username =
                filterContext.HttpContext.User.Claims.SingleOrDefault(x =>
                    x.Type is "Username" or "preferred_username")?.Value ?? "نا مشخص";

        _logger.Log(mongoLog.LogFullData(LogLevel.Error));

        filterContext.Result = new JsonResult(new SamanSalamatResponse
        {
            Message = "Exception",
            IsSuccess = false
        });

        filterContext.ExceptionHandled = true;
    }

    public override Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.ExceptionHandled) return Task.CompletedTask;

        var mongoLog = new MongoLog
        {
            ActionName = context.ActionDescriptor.RouteValues["action"],
            ControllerName = context.ActionDescriptor.RouteValues["controller"],
            ClientIp = context.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString(),
            ServerIp = context.HttpContext.Features.Get<IHttpConnectionFeature>()?.LocalIpAddress?.MapToIPv4().ToString(),
            Exception = context.Exception
        };

        if (context.HttpContext.User.Identity?.IsAuthenticated ?? false)
            mongoLog.Username =
                context.HttpContext.User.Claims.SingleOrDefault(x =>
                        x.Type is "Username" or "preferred_username")?.Value ?? "نا مشخص";

        _logger.Log(mongoLog.LogFullData(LogLevel.Error));

        context.Result = new JsonResult(new SamanSalamatResponse
        {
            Message = "Exception",
            IsSuccess = false
        });

        context.ExceptionHandled = true;

        return Task.CompletedTask;
    }

    #endregion
}