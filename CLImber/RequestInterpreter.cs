using CLImber.Configuration;
using CLImber.Wrappers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CLImber
{
    public interface IRequestInterpreter
    {
        Task<IResponse> HandleRequest(IRequest request);
    }

    public class RequestInterpreter : IRequestInterpreter
    {
        private readonly ClimberConfig _config;
        private readonly ICommandInterpreter _commandInterpreter;

        public RequestInterpreter(ClimberConfig config, ICommandInterpreter commandInterpreter)
        {
            _config = config;
            _commandInterpreter = commandInterpreter;
        }

        public async Task<IResponse> HandleRequest(IRequest request)
        {
            foreach (EndpointConfig endpoint in _config.Endpoints)
            {
                if (IsMatch(request, endpoint, out Match regexMatch))
                {
                    return await _commandInterpreter.Run(endpoint.Command, endpoint.Responses, regexMatch);
                }
            }

            return new UnhandledResponse();
        }

        private bool IsMatch(IRequest request, EndpointConfig endpoint, out Match regexMatch)
        {
            if (!endpoint.Method.Equals(request.Method, System.StringComparison.OrdinalIgnoreCase))
            {
                regexMatch = null;
                return false;
            }

            Regex regex = new Regex($"^(?:{endpoint.Route})$");
            Match match = regex.Match(request.Route);

            regexMatch = match;

            return match.Success;
        }
    }
}
