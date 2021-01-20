using CLImber.Wrappers;
using Microsoft.AspNetCore.Http;
using System;

namespace CLImber.Tests.Wrappers
{
    internal class TestResponse : IResponse
    {
        public void WriteTo(HttpContext httpContext)
        {
            throw new NotSupportedException();
        }
    }
}
