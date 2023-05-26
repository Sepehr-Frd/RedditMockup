﻿using Microsoft.AspNetCore.Mvc.Filters;
using ILogger = Serilog.ILogger;

namespace RedditMockup.Api.Filters;

public class CustomExceptionFilter : ExceptionFilterAttribute
{
    #region [Field(s)]

    private readonly ILogger _logger;

    #endregion

    #region [Constructor]

    public CustomExceptionFilter(ILogger logger) =>
        _logger = logger;

    #endregion

    #region [Overridden Method(s)]

    public override void OnException(ExceptionContext filterContext)
    {
        if (filterContext.ExceptionHandled)
        {
            return;
        }

        _logger.Error(filterContext.Exception, "Unhandled Exception of Type {exceptionType} Thrown", filterContext.Exception.GetType());

        filterContext.ExceptionHandled = true;
    }

    public override Task OnExceptionAsync(ExceptionContext filterContext)
    {
        if (filterContext.ExceptionHandled)
        {
            return Task.CompletedTask;
        }

        _logger.Error(filterContext.Exception, "Unhandled Exception of Type {exceptionType} Thrown", filterContext.Exception.GetType());

        filterContext.ExceptionHandled = true;

        return Task.CompletedTask;
    }

    #endregion
}