using Shop.Catalog.Api.Enums;

namespace Shop.Catalog.Api.Dtos
{
    public class Error
    {
        public ErrorCode Code { get; set; }
        public string Message { get; set; }
    }
}