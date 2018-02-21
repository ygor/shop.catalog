using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Shop.Catalog.Domain.Models;

namespace Shop.Catalog.Infrastructure.Repositories
{
    public interface IProductsRepository
    {
        IEnumerable<Product> GetAll();
        Maybe<Product> GetById(int productId);
    }
}