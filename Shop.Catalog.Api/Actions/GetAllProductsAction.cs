using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.Extensions.Logging;
using Shop.Catalog.Application.Actors;
using Shop.Catalog.Domain.Models;

namespace Shop.Catalog.Api.Actions
{
    public class GetAllProductsAction : IGetAllProductsAction
    {
        private readonly ILogger<GetAllProductsAction> _logger;
        private readonly IActorRef _productsActor;

        public GetAllProductsAction(IProductsActorProvider provider, ILogger<GetAllProductsAction> logger)
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