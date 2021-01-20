using CLImber.Configuration;
using CLImber.Wrappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CLImber
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ClimberConfig climberConfig = _configuration.Get<ClimberConfig>();
            services.AddSingleton(climberConfig);

            services.AddTransient<IRequestInterpreter, RequestInterpreter>();
            services.AddTransient<ICommandInterpreter, CommandInterpreter>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("{*route}", async context =>
                {
                    string route = context.Request.RouteValues["route"]?.ToString() ?? string.Empty;
                    IRequestInterpreter requestInterpreter = app.ApplicationServices.GetRequiredService<IRequestInterpreter>();
                    IResponse response = await requestInterpreter.HandleRequest(new Request(route, context));
                    response.WriteTo(context);
                });
            });
        }
    }
}
