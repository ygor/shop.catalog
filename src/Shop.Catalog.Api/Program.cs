using System;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Health;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;

namespace Shop.Catalog.Api
{
    public static class Program
    {
        private const string OutputTemplate =
            "[{Timestamp:HH:mm:ss} {Level:u3} {TraceIdentifier}] {Message:lj}{NewLine}"; // TODO: move to appsettings

        public static int Main(string[] args)
        {
            SetupLogger();

            try
            {
                Log.Information("Starting web host");
                BuildWebHost(args).Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");

                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void SetupLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: OutputTemplate)
                .CreateLogger();
        }

        private static IWebHost BuildWebHost(string[] args)
        {
            var metrics = AppMetrics.CreateDefaultBuilder()
                .OutputMetrics.AsPrometheusPlainText()
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseSerilog()
                .UseHealth()
                .UseMetricsWebTracking()
                .UseMetrics(
                    options =>
                    {
                        options.EndpointOptions = endpointsOptions =>
                        {
                            endpointsOptions.MetricsTextEndpointOutputFormatter = metrics.OutputMetricsFormatters
                                .GetType<MetricsPrometheusTextOutputFormatter>();
                        };
                    })
                .Build();
        }
    }
}