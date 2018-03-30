using System.Collections.Generic;
using System.Linq;
using Moq;
using Shop.Catalog.Application.Services;
using Shop.Catalog.Domain.Models;
using Shop.Catalog.Infrastructure.Repositories;
using Xunit;

namespace Shop.Catalog.Application.Test.Services
{
    public class ProductsServiceTest
    {
        public ProductsServiceTest()
        {
            _productsRepositoryMock = new Mock<IProductsRepository>();
            _productsRepositoryMock.Setup(mock => mock.GetAll()).Returns(new List<Product> { _product });
        }

        private readonly Mock<IProductsRepository> _productsRepositoryMock;
        private readonly Product _product = new Product();

        [Fact(DisplayName = "Returns all products")]
        public void ReturnsAllProducts()
        {
            var service = new ProductsService(_productsRepositoryMock.Object);
            var products = service.GetAll();

            Assert.IsAssignableFrom<IReadOnlyCollection<Product>>(products);
            Assert.Equal(1, products.Count);
            Assert.Equal(_product, products.First());
        }
    }
}