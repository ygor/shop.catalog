using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shop.Catalog.Api.Dtos;
using Shop.Catalog.Api.Enums;
using Shop.Catalog.Application.Actors;
using Shop.Catalog.Domain.Models;

namespace Shop.Catalog.Api.Actions
{
    public class UpdateStockAction : IUpdateStockAction
    {
        private readonly ILogger<UpdateStockAction> _logger;
        private readonly IActorRef _productsActor;

        public UpdateStockAction(IProductsActorProvider provider, ILogger<UpdateStockAction> logger)
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

            return CreateActionResult(result);
        }

        private IActionResult CreateActionResult(ProductsActor.ProductEvent result)
        {
            switch (result)
            {
                case ProductsActor.StockUpdated stockUpdated:
                    return new OkObjectResult(new Envelope<Product> {Data = stockUpdated.Product});
                case ProductsActor.ProductNotFound _:
                    return new NotFoundObjectResult(CreateErrorResponse(ErrorCode.ProductNotFound));
                case ProductsActor.InsuffientStock _:
                    return new BadRequestObjectResult(CreateErrorResponse(ErrorCode.InsuffientStock));
                default:
                    throw new InvalidEnumArgumentException(nameof(ProductsActor.ProductEvent));
            }
        }

        private Envelope<Product> CreateErrorResponse(ErrorCode code)
        {
            return new Envelope<Product> {Error = new Error {Code = code}};
        }
    }
}