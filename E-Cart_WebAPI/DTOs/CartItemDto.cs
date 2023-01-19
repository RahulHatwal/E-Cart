namespace E_Cart_WebAPI.DTOs
{
    public class CartItemDto
    {
        public int ProductCardId { get; set; }

        public int ProductId { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
