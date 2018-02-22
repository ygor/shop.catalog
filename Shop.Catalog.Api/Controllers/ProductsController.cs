using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Catalog.Api.Actions;
using Shop.Catalog.Api.Dtos;
using Shop.Catalog.Domain.Models;

namespace Shop.Catalog.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("/v{version:apiVersion}/products")]
    public class ProductsController
    {
        private readonly IGetAllProductsAction _getAllProductsAction;
        private readonly IUpdateStockAction _updateStockAction;

        public ProductsController(IGetAllProductsAction getAllProductsAction, IUpdateStockAction updateStockAction)
        {
            _getAllProductsAction = getAllProductsAction;
            _updateStockAction = updateStockAction;
        }

        [HttpGet]
        public async Task<Envelope<IEnumerable<Product>>> GetAsync(CancellationToken cancellationToken)
        {
            return await _getAllProductsAction.ExecuteAsync(cancellationToken);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStockAsync([FromBody] UpdateStockRequest updateStockRequest,
            CancellationToken cancellationToken)
        {
            return await _updateStockAction.ExecuteAsync(updateStockRequest.ProductId, updateStockRequest.AmountChanged,
                cancellationToken);
        }
    }
}