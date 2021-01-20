using CLImber.Configuration;
using CLImber.Wrappers;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CLImber
{
    public interface ICommandInterpreter
    {
        Task<IResponse> Run(string command, ICollection<ResponseConfig> responses, GroupCollection captureGroups);
    }

    public class CommandInterpreter : ICommandInterpreter
    {
        public Task<IResponse> Run(string command, ICollection<ResponseConfig> responses, GroupCollection captureGroups)
        {
            throw new NotImplementedException();
        }
    }
}
