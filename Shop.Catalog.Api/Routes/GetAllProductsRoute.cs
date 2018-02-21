using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.Extensions.Logging;
using Shop.Catalog.Application.Actors;
using Shop.Catalog.Domain.Models;

namespace Shop.Catalog.Api.Routes
{
    public class GetAllProductsRoute : IGetAllProductsRoute
    {
        private readonly ILogger<GetAllProductsRoute> _logger;
        private readonly IActorRef _productsActor;

        public GetAllProductsRoute(IProductsActorProvider provider, ILogger<GetAllProductsRoute> logger)
        {
            _logger = logger;
            _productsActor = provider.Provide();
        }

        public async Task<IEnumerable<Product>> ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Requesting all products");

            return await _productsActor.Ask<IEnumerable<Product>>(
                new ProductsActor.GetAllProducts(),
                cancellationToken
            );
        }
    }
}