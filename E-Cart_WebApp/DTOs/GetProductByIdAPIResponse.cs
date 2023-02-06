using E_Cart_WebApp.Models;

namespace E_Cart_WebApp.DTOs
{
    public class GetProductByIdAPIResponse
    {
        public string Message { get; set; }
        public string Success { get; set; }
        public Product Data { get; set; }
    }
}
