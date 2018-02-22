using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Shop.Catalog.Api.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ErrorCode
    {
        ProductNotFound,
        InsuffientStock
    }
}