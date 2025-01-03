using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SolarEdge.Monitoring.Demo;
using System.IO;

IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args)
  .UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext())
  .UseContentRoot(Directory.GetCurrentDirectory())
  // Disable the dependency injection scope validation feature
  .UseDefaultServiceProvider(options => options.ValidateScopes = false)
  .ConfigureWebHostDefaults(webBuilder =>
  {
    webBuilder.UseStartup<Startup>()
      .UseKestrel();
  })
  .ConfigureAppConfiguration((builderContext, config) =>
  {
    var env = builderContext.HostingEnvironment;
    config.SetBasePath(env.ContentRootPath);
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
    config.AddEnvironmentVariables();
  })
  .ConfigureLogging(logging =>
  {
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddSerilog();
  });

IHost host = hostBuilder.Build();
await host.RunAsync();
