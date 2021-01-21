using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;

namespace CLImber.Tests
{
    public class DependencyInjectionTests
    {
        [Test]
        public void ShouldCorrectlyBuildRequestInterpreter()
        {
            // Arrange
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                { "shell", "bash" }
            }).Build();

            IServiceCollection services = new ServiceCollection();

            Startup startup = new Startup(configuration);
            startup.ConfigureServices(services);

            // Act
            IRequestInterpreter requestInterpreter = services.BuildServiceProvider().GetService<IRequestInterpreter>();

            // Assert
            requestInterpreter.Should().NotBeNull();
        }
    }
}