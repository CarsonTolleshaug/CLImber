using CLImber;
using CLImber.Configuration;
using CLImber.Wrappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

const string ConfigFilename = "climber.yaml";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IRequestInterpreter, RequestInterpreter>();
builder.Services.AddTransient<ICommandInterpreter, CommandInterpreter>();
builder.Services.AddTransient<ICliProcess, CliProcess>();

var climberConfigFile = new StreamReader(ConfigFilename);
var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

ClimberConfig climberConfig = deserializer.Deserialize<ClimberConfig>(climberConfigFile);
builder.Services.AddSingleton(climberConfig);

var app = builder.Build();

app.Map("{*route}", async context =>
{
    string route = context.Request.RouteValues["route"]?.ToString() ?? string.Empty;
    IRequestInterpreter requestInterpreter = app.Services.GetRequiredService<IRequestInterpreter>();
    IResponse response = await requestInterpreter.HandleRequest(new Request(route, context));
    response.WriteTo(context);
});

await app.RunAsync();