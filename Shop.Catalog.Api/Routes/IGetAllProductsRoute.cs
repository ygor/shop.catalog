using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shop.Catalog.Domain.Models;

namespace Shop.Catalog.Api.Routes
{
    public interface IGetAllProductsRoute
    {
        Task<IEnumerable<Product>> ExecuteAsync(CancellationToken cancellationToken);
    }
}