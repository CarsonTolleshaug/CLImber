using NUnit.Framework;
using System.Collections.Generic;
using Moq;
using CLImber.Configuration;
using CLImber.Wrappers;
using FluentAssertions;
using CLImber.Tests.Wrappers;
using Match = System.Text.RegularExpressions.Match;

namespace CLImber.Tests
{
    public class RequestInterpreterTests
    {
        private ClimberConfig _config;
        private Mock<ICommandInterpreter> _mockCommandInterpreter;
        private RequestInterpreter _requestInterpreter;

        private Mock<IRequest> _mockRequest;

        [SetUp]
        public void Setup()
        {
            _config = new ClimberConfig();
            _mockCommandInterpreter = new Mock<ICommandInterpreter>(MockBehavior.Strict);
            _requestInterpreter = new RequestInterpreter(_config, _mockCommandInterpreter.Object);

            _mockRequest = new Mock<IRequest>(MockBehavior.Strict);
        }

        [Test]
        public void GivenNoRoutesMatch_ShouldReturnUnhandledResponse()
        {
            // Arrange
            _config.Endpoints = new List<EndpointConfig>
            {
                new EndpointConfig
                {
                    Route = "foo",
                    Method = "GET"
                }
            };

            _mockRequest.Setup(req => req.Route).Returns("not foo");
            _mockRequest.Setup(req => req.Method).Returns("GET");

            // Act
            IResponse response = _requestInterpreter.HandleRequest(_mockRequest.Object).Result;

            // Assert
            response.Should().BeOfType<UnhandledResponse>();
        }

        [Test]
        public void GivenSimpleRouteMatch_ShouldExecuteCommand()
        {
            // Arrange
            _config.Endpoints = new List<EndpointConfig>
            {
                new EndpointConfig
                {
                    Route = "bar",
                    Method = "GET"
                },
                new EndpointConfig
                {
                    Route = "foo",
                    Method = "GET",
                    Command = "cmd"
                }
            };

            _mockRequest.Setup(req => req.Route).Returns("foo");
            _mockRequest.Setup(req => req.Method).Returns("get");

            _mockCommandInterpreter.Setup(ci => ci.Run("cmd", It.IsAny<ICollection<ResponseConfig>>(), It.IsAny<Match>()))
                .ReturnsAsync(new TestResponse());

            // Act
            IResponse response = _requestInterpreter.HandleRequest(_mockRequest.Object).Result;

            // Assert
            response.Should().BeOfType<TestResponse>();
        }

        [Test]
        public void GivenRouteMatches_ButMethodDoesNot_ShouldReturnUnhandledResponse()
        {
            // Arrange
            _config.Endpoints = new List<EndpointConfig>
            {
                new EndpointConfig
                {
                    Route = "foo",
                    Method = "GET"
                }
            };

            _mockRequest.Setup(req => req.Route).Returns("foo");
            _mockRequest.Setup(req => req.Method).Returns("POST");

            // Act
            IResponse response = _requestInterpreter.HandleRequest(_mockRequest.Object).Result;

            // Assert
            response.Should().BeOfType<UnhandledResponse>();
        }

        [Test]
        public void GivenRegexRouteMatch_ShouldExecuteCommandWithCaptureGroup()
        {
            // Arrange
            _config.Endpoints = new List<EndpointConfig>
            {
                new EndpointConfig
                {
                    Route = "foo/(.*)",
                    Method = "GET",
                    Command = "git"
                }
            };

            _mockRequest.Setup(req => req.Route).Returns("foo/1234");
            _mockRequest.Setup(req => req.Method).Returns("GET");

            _mockCommandInterpreter.Setup(ci => ci.Run("git", It.IsAny<ICollection<ResponseConfig>>(), It.Is<Match>(m => m.Groups[1].Value == "1234")))
                .ReturnsAsync(new TestResponse());

            // Act
            IResponse response = _requestInterpreter.HandleRequest(_mockRequest.Object).Result;

            // Assert
            response.Should().BeOfType<TestResponse>();
        }


        [Test]
        public void GivenRegexWithAlternation_ShouldNotMatchPartial()
        {
            // Arrange
            EndpointConfig matchingEndpoint = new EndpointConfig
            {
                Route = "foo|bar",
                Method = "GET"
            };

            _config.Endpoints = new List<EndpointConfig>
            {
                matchingEndpoint
            };

            _mockRequest.Setup(req => req.Route).Returns("foo and stuff");
            _mockRequest.Setup(req => req.Method).Returns("GET");

            // Act
            IResponse response = _requestInterpreter.HandleRequest(_mockRequest.Object).Result;

            // Assert
            response.Should().BeOfType<UnhandledResponse>();
        }
    }
}
