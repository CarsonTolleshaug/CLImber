using Microsoft.AspNetCore.Http;

namespace CLImber.Wrappers
{
    public interface IResponse
    {
        int StatusCode { get; }
        string Body { get; }
    }

    public class Response : IResponse
    {
        public int StatusCode { get; set; }
        public string Body { get; set; }
    }

    public class UnhandledResponse : IResponse
    {
        public int StatusCode => 500;
        public string Body => string.Empty;
    }

    public static class ResponseExtensions
    {
        public static void WriteTo(this IResponse response, HttpContext httpContext)
        {
            httpContext.Response.StatusCode = response.StatusCode;
            
            if (string.IsNullOrWhiteSpace(response.Body))
            {
                return;
            }

            httpContext.Response.WriteAsync(response.Body);
        }
    }
}
