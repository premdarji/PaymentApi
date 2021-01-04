using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace PaymentAPI.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private IConfiguration configuration;

        public AuthenticationMiddleware(RequestDelegate next,IConfiguration configure)
        {
            _next = next;
            configuration = configure;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            //if (httpContext.Request.Path.Value.StartsWith("/api/Login/ChangePassword"))
            //{
            //    var key = configuration.GetValue<string>("ApplicationSettings:JWT_Secret");
            //    var value = httpContext.Request.Headers["Authorization"];
            //    var handler = new JwtSecurityTokenHandler();
            //    var decodedValue = handler.ReadJwtToken(value);
            //}

            //if (httpContext.Request.Path.Value.StartsWith("/api/Login/ChangePassword"))
            //{

            //    if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            //    {
            //        return AuthenticateResult.Fail("Unauthorized");
            //    }
                   

            //    string authorizationHeader = httpContext.Request.Headers["Authorization"];
            //    if (string.IsNullOrEmpty(authorizationHeader))
            //    {
            //        return AuthenticateResult.NoResult();
            //    }

            //    if (!authorizationHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
            //    {
            //        return AuthenticateResult.Fail("Unauthorized");
            //    }

            //    string token = authorizationHeader.Substring("bearer".Length).Trim();

            //    if (string.IsNullOrEmpty(token))
            //    {
            //        return AuthenticateResult.Fail("Unauthorized");
            //    }

            //    return AuthenticateResult.Fail("Unauthorized");

            //}

            //if (httpContext.Request.Path.Value.StartsWith("/api/Login/Reset"))
            //{

            //    return AuthenticateResult.Fail("Unauthorized");
            //}

            //return AuthenticateResult.Fail("Unauthorized");

            //Pass to the next middleware
            await _next(httpContext);

           // return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
