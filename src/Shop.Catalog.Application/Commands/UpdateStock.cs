namespace Shop.Catalog.Application.Commands
{
    public class UpdateStock : ProductCommand
    {
        public readonly int AmountChanged;
        public readonly int ProductId;

        public UpdateStock(int productId = 0, int amountChanged = 0)
        {
            ProductId = productId;
            AmountChanged = amountChanged;
        }
    }
}