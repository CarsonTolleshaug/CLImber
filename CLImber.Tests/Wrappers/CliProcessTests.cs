using CLImber.Configuration;
using CLImber.Models;
using CLImber.Wrappers;
using FluentAssertions;
using NUnit.Framework;
using System.Runtime.InteropServices;

namespace CLImber.Tests.Wrappers
{
    public class CliProcessTests
    {
        private ClimberConfig _config;
        private ICliProcess _cliProcess;

        [SetUp]
        public void Setup()
        {
            _config = new ClimberConfig();
            _cliProcess = new CliProcess(_config);
        }

        [Test]
        public void ShouldHandleDosCommands()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Inconclusive();
                return;
            }

            // Arrange
            _config.Shell = "cmd";

            // Act
            CliOutput output = _cliProcess.ExecuteAsync("dir").Result;

            // Assert
            output.StdOut.Should().Contain("CLImber.dll");
        }



        [Test]
        public void ShouldHandleBashCommands()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Assert.Inconclusive();
                return;
            }

            // Arrange
            _config.Shell = "bash";

            // Act
            CliOutput output = _cliProcess.ExecuteAsync("ls").Result;

            // Assert
            output.StdOut.Should().Contain("CLImber.dll");
        }
    }
}
