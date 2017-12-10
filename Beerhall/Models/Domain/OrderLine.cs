namespace Beerhall.Models.Domain
{
    public class OrderLine : CartLine
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
    }
}
