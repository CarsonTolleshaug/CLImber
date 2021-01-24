using NUnit.Framework;
using CLImber.Wrappers;
using Moq;
using System.Collections.Generic;
using CLImber.Configuration;
using CLImber.Models;
using FluentAssertions;
using System.Text.RegularExpressions;
using Match = System.Text.RegularExpressions.Match;

namespace CLImber.Tests
{
    public class CommandInterpreterTests
    {
        private Mock<ICliProcess> _mockCliProcess;
        private ICommandInterpreter _commandInterpreter;

        [SetUp]
        public void Setup()
        {
            _mockCliProcess = new Mock<ICliProcess>(MockBehavior.Strict);
            _commandInterpreter = new CommandInterpreter(_mockCliProcess.Object);
        }

        [Test]
        public void GivenNoMatchingResponseExitCode_ShouldExecuteAndReturnUnhandledResponse()
        {
            // Arrange
            const string command = "foo until properly bar-ed";

            List<ResponseConfig> responses = new List<ResponseConfig>
            {
                new ResponseConfig
                {
                    Condition = new ConditionConfig
                    {
                        ExitCode = 0
                    }
                }
            };

            _mockCliProcess.Setup(cp => cp.ExecuteAsync(command))
                .ReturnsAsync(new CliOutput
                {
                    ExitCode = 1
                });

            // Act
            IResponse response = _commandInterpreter.Run(command, responses, null).Result;

            // Assert
            response.Should().BeOfType<UnhandledResponse>();
        }


        [Test]
        public void GivenSimpleMatchingResponse_ShouldExecuteAndGenerateResponse()
        {
            // Arrange
            const string command = "foo until properly bar-ed";

            List<ResponseConfig> responses = new List<ResponseConfig>
            {
                new ResponseConfig
                {
                    Condition = new ConditionConfig
                    {
                        ExitCode = 0
                    },
                    ResponseCode = 418,
                    ResponseBody = new
                    {
                        aTeaPot = "I am"
                    }
                }
            };

            _mockCliProcess.Setup(cp => cp.ExecuteAsync(command))
                .ReturnsAsync(new CliOutput
                {
                    ExitCode = 0
                });

            // Act
            IResponse response = _commandInterpreter.Run(command, responses, null).Result;

            // Assert
            response.StatusCode.Should().Be(418);
            response.Body.Should().Be("{\"aTeaPot\":\"I am\"}");
        }

        [Test]
        public void GivenRegexMatchingResponse_ShouldExecuteAndGenerateResponse()
        {
            // Arrange
            const string command = "foo $1";
            const string _1 = "bar";

            List<ResponseConfig> responses = new List<ResponseConfig>
            {
                new ResponseConfig
                {
                    Condition = new ConditionConfig
                    {
                        ExitCode = 0
                    },
                    ResponseCode = 418,
                    ResponseBody = new
                    {
                        aTeaPot = "I am"
                    }
                }
            };

            _mockCliProcess.Setup(cp => cp.ExecuteAsync($"foo {_1}"))
                .ReturnsAsync(new CliOutput
                {
                    ExitCode = 0
                });

            Match match = new Regex("(.*)").Match(_1);

            // Act
            IResponse response = _commandInterpreter.Run(command, responses, match).Result;

            // Assert
            response.StatusCode.Should().Be(418);
            response.Body.Should().Be("{\"aTeaPot\":\"I am\"}");
        }
    }
}
