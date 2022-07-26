using CLImber.Configuration;
using CLImber.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CLImber.Wrappers
{
    public interface ICliProcess
    {
        Task<CliOutput> Execute(string command);
    }

    public class CliProcess : ICliProcess
    {
        private readonly ClimberConfig _config;
        private readonly Dictionary<string, string> _shellsPaths;

        public CliProcess(ClimberConfig config)
        {
            _config = config;
            _shellsPaths = new Dictionary<string, string>
            {
                { "cmd", "cmd.exe" },
                { "bash", "/bin/bash" }
            };
        }

        public async Task<CliOutput> Execute(string command)
        {
            Process shell = new Process();
            shell.StartInfo.FileName = _shellsPaths[_config.Shell];
            shell.StartInfo.RedirectStandardInput = true;
            shell.StartInfo.RedirectStandardError = true;
            shell.StartInfo.RedirectStandardOutput = true;
            shell.StartInfo.CreateNoWindow = true;
            shell.StartInfo.UseShellExecute = false;
            shell.Start();

            shell.StandardInput.WriteLine(command);
            shell.StandardInput.Flush();
            shell.StandardInput.Close();
            shell.WaitForExit();

            return new CliOutput
            {
                ExitCode = shell.ExitCode,
                StdOut = await shell.StandardOutput.ReadToEndAsync(),
                StdErr = await shell.StandardError.ReadToEndAsync()
            };
        }
    }
}
