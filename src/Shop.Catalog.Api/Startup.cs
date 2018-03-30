using Akka.Actor;
using App.Metrics;
using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.Catalog.Api.Actions;
using Shop.Catalog.Api.Actions.Contracts;
using Shop.Catalog.Api.Middleware;
using Shop.Catalog.Application.Actors;
using Shop.Catalog.Application.Actors.Contracts;
using Shop.Catalog.Application.Services;
using Shop.Catalog.Application.Services.Contracts;
using Shop.Catalog.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.Swagger;

namespace Shop.Catalog.Api
{
    public class Startup
    {
        private const string Name = "Shop.Catalog";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IActorRefFactory>(_ =>
                ActorSystem.Create(Name.Replace(".", "-").ToLowerInvariant()));

            services.AddSingleton<IProductsRepository, ProductsRepository>();
            services.AddSingleton<IProductsActorProvider, ProductsActorProvider>();
            services.AddSingleton<IGetAllProductsAction, GetAllProductsAction>();
            services.AddSingleton<IUpdateStockAction, UpdateStockAction>();
            services.AddSingleton<IProductsService, ProductsService>();

            services
                .AddMvcCore(options => options.AddMetricsResourceFilter())
                .AddJsonFormatters()
                .AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");

            services.AddCorrelationId();
            services.AddApiVersioning();
            services.AddSwaggerGen(
                options =>
                {
                    var provider = services.BuildServiceProvider()
                        .GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var description in provider.ApiVersionDescriptions)
                        options.SwaggerDoc(
                            description.GroupName,
                            new Info
                            {
                                Title = $"{Name} API {description.ApiVersion}",
                                Version = description.ApiVersion.ToString()
                            });
                });
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseCorrelationId();
            app.UseMiddleware<TraceIdentifierLoggingMiddleware>();

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                });
        }
    }
}