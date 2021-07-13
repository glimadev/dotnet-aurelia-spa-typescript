using Microsoft.AspNetCore.Http;

namespace AureliaWithDotNet.Domain.Extensions
{
    public static class HttpContextAccessorExtension
    {
        public static string GetURL(this IHttpContextAccessor httpcontextaccessor)
        {
            var request = httpcontextaccessor.HttpContext.Request;

            var absoluteUri = string.Concat(
                        request.Scheme,
                        "://",
                        request.Host.ToUriComponent());

            return absoluteUri;
        }
    }
}
