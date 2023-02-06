using E_Cart_WebApp.Models;

namespace E_Cart_WebApp.DTOs
{
    public class GetProductsAPIResponse
    {
        public string Message { get; set; }
        public string Success { get; set; }
        public List<Product> Data { get; set; }
    }
}
