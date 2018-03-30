using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Shop.Catalog.Domain.Models;

namespace Shop.Catalog.Infrastructure.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly IEnumerable<Product> _products = new List<Product>
        {
            new Product
            {
                Id = 1000,
                Title = "Playstation 4 500GB",
                Brand = "Sony",
                PricePerUnit = 29900,
                InStock = 5
            },
            new Product
            {
                Id = 1001,
                Title = "Playstation 4 Pro 1TB",
                Brand = "Sony",
                PricePerUnit = 39900,
                InStock = 2
            },
            new Product
            {
                Id = 1002,
                Title = "XBOX One",
                Brand = "Microsoft",
                PricePerUnit = 26700,
                InStock = 10
            },
            new Product
            {
                Id = 1003,
                Title = "XBOX One Scorpio",
                Brand = "Microsoft",
                PricePerUnit = 499000,
                InStock = 1
            },
            new Product
            {
                Id = 1004,
                Title = "Wii U",
                Brand = "Nintendo",
                PricePerUnit = 19900,
                InStock = 8
            },
            new Product
            {
                Id = 1005,
                Title = "Switch",
                Brand = "Nintendo",
                PricePerUnit = 23500,
                InStock = 1
            }
        };

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Maybe<Product> GetById(int productId)
        {
            return _products.FirstOrDefault(product => product.Id == productId);
        }
    }
}