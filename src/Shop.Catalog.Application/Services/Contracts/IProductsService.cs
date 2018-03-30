using System.Collections.Generic;
using Shop.Catalog.Application.Commands;
using Shop.Catalog.Application.Events;
using Shop.Catalog.Domain.Models;

namespace Shop.Catalog.Application.Services.Contracts
{
    public interface IProductsService
    {
        IReadOnlyCollection<Product> GetAll();
        ProductEvent UpdateStock(UpdateStock message);
    }
}