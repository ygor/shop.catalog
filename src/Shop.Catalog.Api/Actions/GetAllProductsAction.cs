using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.Extensions.Logging;
using Shop.Catalog.Api.Actions.Contracts;
using Shop.Catalog.Api.Dtos;
using Shop.Catalog.Application.Actors;
using Shop.Catalog.Application.Actors.Contracts;
using Shop.Catalog.Application.Commands;
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

        public async Task<Envelope<IEnumerable<Product>>> ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Requesting all products");

            var products = await _productsActor.Ask<IEnumerable<Product>>(
                new GetAllProducts(),
                cancellationToken
            );

            return new Envelope<IEnumerable<Product>> {Data = products};
        }
    }
}