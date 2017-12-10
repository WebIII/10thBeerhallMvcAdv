using Newtonsoft.Json;

namespace Beerhall.Models.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CartLine
    {
        [JsonProperty]
        public int Quantity { get; set; }
        [JsonProperty]
        public Beer Product { get; set; }
        public decimal Total => Product.Price * Quantity;
    }
}