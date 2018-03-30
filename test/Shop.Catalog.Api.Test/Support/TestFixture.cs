using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shop.Catalog.Api.Test.Support
{
    public class TestFixture<TStartup> : IDisposable where TStartup : class
    {
        public TestFixture(Action<IServiceCollection> configureServices)
            : this(Path.Combine("src"), configureServices)
        {
        }

        protected TestFixture(string relativeTargetProjectParentDir, Action<IServiceCollection> configureServices)
        {
            CultureInfo.CurrentCulture = new CultureInfo("nl-NL");
            var environment = "Development";

            var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(relativeTargetProjectParentDir, startupAssembly);
            var config = new ConfigurationBuilder()
                .SetBasePath(contentRoot)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json")
                .Build();

            var builder = new WebHostBuilder()
                .UseContentRoot(contentRoot)
                .UseConfiguration(config)
                .ConfigureMetrics()
                .ConfigureServices(configureServices)
                .UseEnvironment(environment)
                .UseStartup(typeof(TStartup));

            Server = new TestServer(builder);
            Client = Server.CreateClient();
        }

        public TestServer Server { get; }
        public HttpClient Client { get; }

        private string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
        {
            var projectName = startupAssembly.GetName().Name;
            var applicationBasePath = AppContext.BaseDirectory;
            var directoryInfo = new DirectoryInfo(applicationBasePath);

            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));
                if (projectDirectoryInfo.Exists)
                {
                    var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName,
                        $"{projectName}.csproj"));

                    if (projectFileInfo.Exists) return Path.Combine(projectDirectoryInfo.FullName, projectName);
                }
            } while (directoryInfo.Parent != null);

            throw new DirectoryNotFoundException(
                $"Project root could not be located using the application root {applicationBasePath}.");
        }

        public void Dispose()
        {
            Server.Dispose();
            Client.Dispose();
        }
    }
}