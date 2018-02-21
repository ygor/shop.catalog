using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Catalog.Api.Routes
{
    public interface IUpdateStockRoute
    {
        Task<IActionResult> ExecuteAsync(int productId, int amountChanged, CancellationToken cancellationToken);
    }
}