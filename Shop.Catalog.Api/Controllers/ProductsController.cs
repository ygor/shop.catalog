using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Catalog.Api.Dtos;
using Shop.Catalog.Api.Routes;
using Shop.Catalog.Domain.Models;

namespace Shop.Catalog.Api.Controllers
{
    [Route("/products")]
    public class ProductsController
    {
        private readonly IGetAllProductsRoute _getAllProductsRoute;
        private readonly IUpdateStockRoute _updateStockRoute;

        public ProductsController(IGetAllProductsRoute getAllProductsRoute, IUpdateStockRoute updateStockRoute)
        {
            _getAllProductsRoute = getAllProductsRoute;
            _updateStockRoute = updateStockRoute;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetAsync(CancellationToken cancellationToken)
        {
            return await _getAllProductsRoute.ExecuteAsync(cancellationToken);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStockAsync([FromBody] UpdateStock updateStock,
            CancellationToken cancellationToken)
        {
            return await _updateStockRoute.ExecuteAsync(updateStock.ProductId, updateStock.AmountChanged,
                cancellationToken);
        }
    }
}