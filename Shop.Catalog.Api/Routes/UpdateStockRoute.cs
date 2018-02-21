using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shop.Catalog.Application.Actors;

namespace Shop.Catalog.Api.Routes
{
    public class UpdateStockRoute : IUpdateStockRoute
    {
        private readonly ILogger<UpdateStockRoute> _logger;
        private readonly IActorRef _productsActor;

        public UpdateStockRoute(IProductsActorProvider provider, ILogger<UpdateStockRoute> logger)
        {
            _logger = logger;
            _productsActor = provider.Provide();
        }

        public async Task<IActionResult> ExecuteAsync(int productId, int amountChanged,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Changing stock by {amountChanged} for product {productId}.");

            var result = await _productsActor.Ask<ProductsActor.ProductEvent>(
                new ProductsActor.UpdateStock(productId, amountChanged),
                cancellationToken
            );

            if (result is ProductsActor.ProductNotFound || result is ProductsActor.InsuffientStock)
                return new BadRequestResult();

            return new OkResult();
        }
    }
}