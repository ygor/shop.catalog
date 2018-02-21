using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Akka.Actor;
using Shop.Catalog.Domain.Models;
using Shop.Catalog.Infrastructure.Repositories;

namespace Shop.Catalog.Application.Actors
{
    public partial class ProductsActor : ReceiveActor
    {
        private readonly IProductsRepository _repository;

        public ProductsActor(IProductsRepository repository)
        {
            _repository = repository;

            Receive<GetAllProducts>(_ => Sender.Tell(GetAll()));
            Receive<UpdateStock>(message => Sender.Tell(OnUpdateStock(message)));
        }

        public IReadOnlyCollection<Product> GetAll()
        {
            return new ReadOnlyCollection<Product>(_repository.GetAll().ToList());
        }

        public ProductEvent OnUpdateStock(UpdateStock message)
        {
            var maybeProduct = _repository.GetById(message.ProductId);

            if (!maybeProduct.HasValue) return new ProductNotFound();

            var product = maybeProduct.Value;
            if (product.InStock + message.AmountChanged >= 0)
            {
                product.InStock += message.AmountChanged;
                return new StockUpdated(product);
            }

            return new InsuffientStock();
        }
    }
}