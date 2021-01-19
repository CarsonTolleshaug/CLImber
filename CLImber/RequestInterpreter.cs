using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CLImber
{
    public interface IRequestInterpreter
    {
        Task HandleRequest(string route, HttpContext context);
    }

    public class RequestInterpreter : IRequestInterpreter
    {
        public Task HandleRequest(string route, HttpContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
