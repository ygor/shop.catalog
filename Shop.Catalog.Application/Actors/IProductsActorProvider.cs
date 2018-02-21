using Akka.Actor;

namespace Shop.Catalog.Application.Actors
{
    public interface IProductsActorProvider
    {
        IActorRef Provide();
    }
}