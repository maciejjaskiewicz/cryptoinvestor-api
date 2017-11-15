using CryptoInvestor.Core.Exceptions;
using CryptoInvestor.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CryptoInvestor.Api.Framework
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorCode = "error";
            var statusCode = HttpStatusCode.BadRequest;
            var exceptionMassage = "Something went wrong!";

            var exceptionType = exception.GetType();

            switch (exception)
            {
                case Exception e when exceptionType == typeof(UnauthorizedAccessException):
                    exceptionMassage = "Unauthorized access.";
                    statusCode = HttpStatusCode.Unauthorized;
                    break;

                case DomainException e when exceptionType == typeof(DomainException):
                    exceptionMassage = e.Message;
                    statusCode = HttpStatusCode.BadRequest;
                    errorCode = e.Code;
                    break;

                case ServiceException e when exceptionType == typeof(ServiceException):
                    exceptionMassage = e.Message;
                    statusCode = HttpStatusCode.BadRequest;
                    errorCode = e.Code;
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            var response = new
            {
                code = errorCode,
                message = exceptionMassage
            };

            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(payload);
        }
    }
}