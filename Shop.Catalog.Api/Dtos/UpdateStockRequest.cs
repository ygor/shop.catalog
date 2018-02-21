namespace Shop.Catalog.Api.Dtos
{
    public class UpdateStockRequest
    {
        public int ProductId { get; set; }
        public int AmountChanged { get; set; }
    }
}