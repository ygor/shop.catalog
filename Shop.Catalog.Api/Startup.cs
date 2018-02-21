using System.Buffers;
using Akka.Actor;
using JsonApiSerializer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Shop.Catalog.Api.Actions;
using Shop.Catalog.Application.Actors;
using Shop.Catalog.Infrastructure.Repositories;

namespace Shop.Catalog.Api
{
    public class Startup
    {
        private const string Name = "catalog-service";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddJsonFormatters();

            services.AddSingleton<IActorRefFactory>(_ => ActorSystem.Create(Name));

            services.AddSingleton<IProductsRepository, ProductsRepository>();
            services.AddSingleton<IProductsActorProvider, ProductsActorProvider>();
            services.AddSingleton<IGetAllProductsAction, GetAllProductsAction>();
            services.AddSingleton<IUpdateStockAction, UpdateStockAction>();

            services.AddMvc(opt =>
            {
                var sp = services.BuildServiceProvider();
                var logger = sp.GetService<ILoggerFactory>();
                var objectPoolProvider = sp.GetService<ObjectPoolProvider>();

                var serializerSettings = new JsonApiSerializerSettings();

                var jsonApiFormatter = new JsonOutputFormatter(serializerSettings, ArrayPool<char>.Shared);
                opt.OutputFormatters.RemoveType<JsonOutputFormatter>();
                opt.OutputFormatters.Insert(0, jsonApiFormatter);

                var jsonApiInputFormatter = new JsonInputFormatter(logger.CreateLogger<JsonInputFormatter>(),
                    serializerSettings, ArrayPool<char>.Shared, objectPoolProvider);
                opt.InputFormatters.RemoveType<JsonInputFormatter>();
                opt.InputFormatters.Insert(0, jsonApiInputFormatter);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}