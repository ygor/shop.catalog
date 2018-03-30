namespace Shop.Catalog.Api.Dtos
{
    public class Envelope<TData>
    {
        public TData Data { get; set; }
        public Error Error { get; set; }
    }
}