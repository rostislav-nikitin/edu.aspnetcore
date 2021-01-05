using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExampleWebApp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private IServiceCollection _services;

        public Startup(IHostEnvironment env)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();

            builder.SetBasePath(env.ContentRootPath);
            builder.AddInMemoryCollection(new Dictionary<string, string>
            {
                {"Config::InMemoryKey", "Value"},
                {"ApplicationName", null},
                {"EnvironmentName", null}
            });

            builder.AddJsonFile("Custom.Config.json");
            builder.AddXmlFile("Custom.Config.xml", false, true);
            builder.AddXmlFile("Custom.Optional.Config.xml", true);

            _configuration = builder.Build();
        }
        

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICustomService, CustomService>();
            services.AddRouting();

            _services = services;
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ICustomService customService)
        {
            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Startup>();

            logger.LogInformation("App configuring");

            _configuration["ApplicationName"] = env.ApplicationName;
            _configuration["EnvironmentName"] = env.ContentRootPath;
                    
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseMiddleware<CustomMiddleware>();

            int counter = 0;

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/Home", async context => 
                {
                    await context.Response.WriteAsync("<h1>Home</h1>\n");
                });
                endpoints.MapGet("{controller}/{action}", async context =>
                {
                    var controller = context.GetRouteValue("controller");
                    var action = context.GetRouteValue("action");

                    await context.Response.WriteAsync($"{controller}/{action} route was used");
                    
                });
                endpoints.MapGet("{controller=Home}/{action}/{id}", async context =>
                {
                    var controller = context.GetRouteValue("controller");
                    var action = context.GetRouteValue("action");
                    var id = context.GetRouteValue("id");

                    await context.Response.WriteAsync($"{controller}/{action}/{id} route was used");
                });

                endpoints.MapGet("{controller=Home}/{action}/{id}/{name}/{value?}", async context =>
                {
                    await context.Response.WriteAsync("{controller}/{action}/{id}/{name}/{value?} route was used");
                });

                endpoints.MapGet("{controller=Home}/{action}/{id}/{name}/{value}/{*catchall}", async context =>
                {
                    RouteData routeData = context.GetRouteData();

                    await context.Response.WriteAsync("<h2>RouteData.Values</h2>");
                    foreach(var pair in routeData.Values)
                        await context.Response.WriteAsync($"{pair.Key}: {pair.Value} <br />\n");

                    await context.Response.WriteAsync("<h2>RouteData.Routers</h2>");
                    foreach(var router in routeData.Routers)
                        await context.Response.WriteAsync($"{router.ToString()}<br />\n");

                    await context.Response.WriteAsync("{controller}/{action}/{id}/{name}/{value}/{*catchall} route was used");
                });

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("<h1>Counter</h1>\n");
                    await context.Response.WriteAsync($"Hello World!\ncounter: {counter++}\n");
                    await context.Response.WriteAsync("<h1>Configuration</h1>\n");

                    await context.Response.WriteAsync("<h2>Base configuration</h2>\n");
                    await context.Response.WriteAsync($"Custom Config::InMemoryKey = {_configuration["Config::InMemoryKey"]}<br />\n");
                    await context.Response.WriteAsync($"ApplicationName: {_configuration["ApplicationName"]}<br />\n");
                    await context.Response.WriteAsync($"EnvironmentName: {_configuration["EnvironmentName"]}<br />\n");

                    await context.Response.WriteAsync("<h2>JSON configuration</h2>\n");
                    await context.Response.WriteAsync("<h3>Main section</h2>\n");
                    await context.Response.WriteAsync($"JsonKeyForString = {_configuration["JsonKeyForString"]}<br />\n");
                    await context.Response.WriteAsync($"JsonKeyForInt = {_configuration["JsonKeyForInt"]}<br />\n");
                    await context.Response.WriteAsync("<h3>Custom section</h2>\n");
                    IConfigurationSection customSection = _configuration.GetSection("CustomSection");
                    await context.Response.WriteAsync($"CustomSectionKey = {customSection["CustomSectionKey"]}<br />\n");
                    await context.Response.WriteAsync("<h3>Custom section with sub sections</h2>\n");
                    IConfigurationSection customSectionWithSubSection = 
                        _configuration.GetSection("CustomSectionWithSubSections");
                    IEnumerable<IConfigurationSection> subSections = 
                        customSectionWithSubSection.GetChildren();
                    await context.Response.WriteAsync($"<ul>");
                    foreach(IConfigurationSection subSection in subSections)
                    {
                        await context.Response.WriteAsync($"<li>SubSectionKey = {subSection["SubSectionKey"]}</li>\n");
                    }
                    await context.Response.WriteAsync($"</ul>");
                    await context.Response.WriteAsync("<h2>Command line configuration</h2>\n");
                    await context.Response.WriteAsync("<h2>Environment variables configuration</h2>\n");
                    await context.Response.WriteAsync("<h2>Ini configuration</h2>\n");
                    await context.Response.WriteAsync("<h2>XML configuration</h2>\n");
                    await context.Response.WriteAsync($"XmlKeyForString = {_configuration["XmlKeyForString"]}<br />\n");
                    await context.Response.WriteAsync($"XmlKeyForInt = {_configuration["XmlKeyForInt"]}<br />\n");
                    await context.Response.WriteAsync("<h1>Services</h1>\n");
                    await context.Response.WriteAsync("<h2>Registered services</h2>\n");
                    await context.Response.WriteAsync($"<ul>");
                    foreach(var service in _services)
                    {
                        await context.Response.WriteAsync($"<li>Service Type = {service.ServiceType.ToString()}, Impl. Type = {service.ImplementationType?.ToString()}, Lifetime = {service.Lifetime.ToString()}</li>\n");
                    }
                    await context.Response.WriteAsync($"</ul>");
                    await context.Response.WriteAsync("<h2>Custom service</h2>\n");
                    await context.Response.WriteAsync("<h3>From parameters</h2>\n");
                    await context.Response.WriteAsync($"Time is: {customService.GetTime()}<br />\n");
                    await context.Response.WriteAsync("<h3>From app.ApplicationServices</h2>\n");
                    ICustomService customServiceFromApp = app.ApplicationServices.GetService<ICustomService>();
                    await context.Response.WriteAsync($"Time is: {customServiceFromApp.GetTime()}<br />\n");
                    
                });
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Default route.");
            });

        }
    }
}
