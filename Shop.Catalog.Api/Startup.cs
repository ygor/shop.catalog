using Akka.Actor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shop.Catalog.Api.Actions;
using Shop.Catalog.Application.Actors;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddJsonFormatters().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");

            services.AddMvc(options => options.AddMetricsResourceFilter());
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

            services.AddSingleton<IActorRefFactory>(_ => ActorSystem.Create(Name));

            services.AddSingleton<IProductsRepository, ProductsRepository>();
            services.AddSingleton<IProductsActorProvider, ProductsActorProvider>();
            services.AddSingleton<IGetAllProductsAction, GetAllProductsAction>();
            services.AddSingleton<IUpdateStockAction, UpdateStockAction>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

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