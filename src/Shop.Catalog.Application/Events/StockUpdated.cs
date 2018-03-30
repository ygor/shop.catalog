using Shop.Catalog.Domain.Models;

namespace Shop.Catalog.Application.Events
{
    public class StockUpdated : ProductEvent
    {
        public readonly Product Product;

        public StockUpdated(Product product)
        {
            Product = product;
        }
    }
}