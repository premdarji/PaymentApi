using LoggerService;
using Microsoft.AspNetCore.Http;
using Payment.Domain;
using Payment.Entity;
using Payment.Entity.ViewModels;
using PaymentAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PaymentAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;
        //private ICommonDomain common;
        // ApplicationContext _context;
        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _logger = logger;
            _next = next;
          
           
            
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ErrorDetails error = new ErrorDetails();
            error.StatusCode = (int)HttpStatusCode.InternalServerError;
            error.Message = Convert.ToString(exception);

            //ErrorDomain er = new ErrorDomain();
            //er.RegisterError(error);

            // common.RegisterError(error);

            //_context.Errordetails.Add(error);
            //_context.SaveChanges();

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;


            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = Convert.ToString(exception)
            }.ToString());
        }
    }
}
