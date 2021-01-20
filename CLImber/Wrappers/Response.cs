using Microsoft.AspNetCore.Http;
using System;

namespace CLImber.Wrappers
{
    public interface IResponse
    {
        void WriteTo(HttpContext httpContext);
    }

    public class Response : IResponse
    {
        public void WriteTo(HttpContext httpContext)
        {
            throw new NotImplementedException();
        }
    }

    public class UnhandledResponse : IResponse
    {
        public void WriteTo(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 500;
        }
    }
}
