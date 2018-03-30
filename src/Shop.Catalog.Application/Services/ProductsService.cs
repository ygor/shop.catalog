using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Shop.Catalog.Application.Commands;
using Shop.Catalog.Application.Events;
using Shop.Catalog.Application.Services.Contracts;
using Shop.Catalog.Domain.Models;
using Shop.Catalog.Infrastructure.Repositories;

namespace Shop.Catalog.Application.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _repository;

        public ProductsService(IProductsRepository repository)
        {
            _repository = repository;
        }

        public IReadOnlyCollection<Product> GetAll()
        {
            return new ReadOnlyCollection<Product>(_repository.GetAll().ToList());
        }

        public ProductEvent UpdateStock(UpdateStock message)
        {
            var maybeProduct = _repository.GetById(message.ProductId);

            if (!maybeProduct.HasValue) return new ProductNotFound();

            var product = maybeProduct.Value;
            if (product.InStock + message.AmountChanged >= 0)
            {
                product.InStock += message.AmountChanged;
                return new StockUpdated(product);
            }

            return new InsufficientStock();
        }
    }
}