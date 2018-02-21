using Shop.Catalog.Domain.Models;

namespace Shop.Catalog.Application.Actors
{
    public partial class ProductsActor
    {
        public abstract class ProductEvent
        {
        }

        public class ProductNotFound : ProductEvent
        {
        }

        public class StockUpdated : ProductEvent
        {
            public readonly Product Product;

            public StockUpdated(Product product)
            {
                Product = product;
            }
        }

        public class InsuffientStock : ProductEvent
        {
        }
    }
}