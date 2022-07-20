using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace RedditMockup.Api.Filters;

//public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
//{
//    private readonly string[] _roles;

//    public AuthorizationFilter(string[] roles) =>
//        _roles = roles;

//    public void OnAuthorization(AuthorizationFilterContext context)
//    {

//        if (context.ActionDescriptor.EndpointMetadata.Contains(typeof(AllowAnonymousAttribute)))
//            return;

//        if (context.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier) != null)
//            return;

//        else
//        {
//            context.HttpContext.Response.StatusCode = 403;
//            context.HttpContext.Response.Body = Stream.Null;
//        }
//    }
//}