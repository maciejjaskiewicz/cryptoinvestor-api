using Microsoft.AspNetCore.Builder;

namespace CryptoInvestor.Api.Framework
{
    public static class MiddlewereExtensions
    {
        public static IApplicationBuilder UseExceptionsHandler(this IApplicationBuilder builder) =>
            builder.UseMiddleware(typeof(ExceptionHandlerMiddleware));
    }
}