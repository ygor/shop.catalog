﻿using Akka.Actor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}