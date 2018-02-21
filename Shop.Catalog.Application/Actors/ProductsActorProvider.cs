using Akka.Actor;
using Shop.Catalog.Infrastructure.Repositories;

namespace Shop.Catalog.Application.Actors
{
    public class ProductsActorProvider : IProductsActorProvider
    {
        private const string Name = "products";
        private readonly IActorRef _productsActor;

        public ProductsActorProvider(IActorRefFactory actorSystem, IProductsRepository productsRepository)
        {
            _productsActor = actorSystem.ActorOf(Props.Create<ProductsActor>(productsRepository), Name);
        }

        public IActorRef Provide()
        {
            return _productsActor;
        }
    }
}