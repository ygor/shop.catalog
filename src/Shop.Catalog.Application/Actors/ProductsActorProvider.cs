using Akka.Actor;
using Shop.Catalog.Application.Actors.Contracts;
using Shop.Catalog.Application.Services.Contracts;

namespace Shop.Catalog.Application.Actors
{
    public class ProductsActorProvider : IProductsActorProvider
    {
        private const string Name = "products";
        private readonly IActorRef _productsActor;

        public ProductsActorProvider(IActorRefFactory actorSystem, IProductsService productsService)
        {
            _productsActor = actorSystem.ActorOf(Props.Create<ProductsActor>(productsService), Name);
        }

        public IActorRef Provide()
        {
            return _productsActor;
        }
    }
}