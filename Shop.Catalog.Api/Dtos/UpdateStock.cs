namespace Shop.Catalog.Api.Dtos
{
    public class UpdateStock
    {
        public int ProductId { get; set; }
        public int AmountChanged { get; set; }
    }
}