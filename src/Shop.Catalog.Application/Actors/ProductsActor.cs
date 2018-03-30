using Akka.Actor;
using Shop.Catalog.Application.Commands;
using Shop.Catalog.Application.Services.Contracts;

namespace Shop.Catalog.Application.Actors
{
    public class ProductsActor : ReceiveActor
    {
        public ProductsActor(IProductsService productsService)
        {
            Receive<GetAllProducts>(_ => Sender.Tell(productsService.GetAll()));
            Receive<UpdateStock>(message => Sender.Tell(productsService.UpdateStock(message)));
        }
    }
}