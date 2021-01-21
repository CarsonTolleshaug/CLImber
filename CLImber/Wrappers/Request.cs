using Microsoft.AspNetCore.Http;

namespace CLImber.Wrappers
{
    public interface IRequest
    {
        string Route { get; }
        string Method { get; }
    }

    public class Request : IRequest
    {
        private readonly string _route;
        private readonly HttpContext _httpContext;

        public Request(string route, HttpContext httpContext)
        {
            _route = route;
            _httpContext = httpContext;
        }

        public string Route => _route;
        public string Method => _httpContext.Request.Method;
    }
}
