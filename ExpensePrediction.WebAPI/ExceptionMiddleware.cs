using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ExpensePrediction.DataTransferObjects;
using ApplicationException = ExpensePrediction.Exceptions.ApplicationException;

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
            catch (ApplicationException e)
            {
                await HandleExceptionAsync(context, e, e.HttpCode, e.Code);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e, 500, "unknown_error");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, int code, string error)
        {
            context.Response.ContentType = Constants.ApplicationJson;
            context.Response.StatusCode = code;

            var response = new ErrorDto { ErrorCode = error, Message = exception.Message };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
