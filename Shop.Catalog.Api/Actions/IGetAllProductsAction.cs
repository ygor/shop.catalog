using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shop.Catalog.Api.Dtos;
using Shop.Catalog.Domain.Models;

namespace Shop.Catalog.Api.Actions
{
    public interface IGetAllProductsAction
    {
        Task<Envelope<IEnumerable<Product>>> ExecuteAsync(CancellationToken cancellationToken);
    }
}