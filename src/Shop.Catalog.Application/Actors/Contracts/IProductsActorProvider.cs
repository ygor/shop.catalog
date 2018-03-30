using Akka.Actor;

namespace Shop.Catalog.Application.Actors.Contracts
{
    public interface IProductsActorProvider
    {
        IActorRef Provide();
    }
}