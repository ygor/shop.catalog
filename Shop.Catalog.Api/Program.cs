﻿using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Health;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Shop.Catalog.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseHealth()
                .UseMetrics()
                .Build();
        }
    }
}