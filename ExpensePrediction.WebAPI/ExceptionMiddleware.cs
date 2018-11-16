using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ExpensePrediction.WebAPI
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            //TODO Catch Service and Repository exceptions
            catch (Exception e)
            {
                //TODO consider logging
                await HandleExceptionAsync(context, e, 500);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, int code)
        {
            context.Response.ContentType = Constants.ApplicationJson;
            context.Response.StatusCode = code;

            var response = new { ErrorCode = code, Message = exception.Message };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
