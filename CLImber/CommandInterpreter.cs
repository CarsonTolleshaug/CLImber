using CLImber.Configuration;
using CLImber.Models;
using CLImber.Wrappers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CLImber
{
    public interface ICommandInterpreter
    {
        Task<IResponse> Run(string command, ICollection<ResponseConfig> responses, Match regexMatch);
    }

    public class CommandInterpreter : ICommandInterpreter
    {
        private ICliProcess _cliProcess;

        public CommandInterpreter(ICliProcess cliProcess)
        {
            _cliProcess = cliProcess;
        }

        public async Task<IResponse> Run(string command, ICollection<ResponseConfig> responses, Match regexMatch)
        {
            string fullCommand = regexMatch == null ? command : regexMatch.Result(command);

            CliOutput output = await _cliProcess.Execute(fullCommand);

            foreach (ResponseConfig responseConfig in responses)
            {
                if (output.ExitCode == responseConfig.Condition.ExitCode)
                {
                    return new Response
                    {
                        StatusCode = responseConfig.ResponseCode,
                        Body = JsonConvert.SerializeObject(responseConfig.ResponseBody)
                    };
                }
            }

            return new UnhandledResponse();
        }
    }
}
