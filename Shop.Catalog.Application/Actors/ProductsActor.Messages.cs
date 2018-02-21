namespace Shop.Catalog.Application.Actors
{
    public partial class ProductsActor
    {
        public class GetAllProducts {}

        public class UpdateStock
        {
            public readonly int ProductId;
            public readonly int AmountChanged;

            public UpdateStock(int productId = 0, int amountChanged = 0)
            {
                ProductId = productId;
                AmountChanged = amountChanged;
            }
        }
    }
}
