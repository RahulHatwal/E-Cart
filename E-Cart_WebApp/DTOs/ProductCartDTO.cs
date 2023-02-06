using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Cart_WebApp.DTOs
{
    
        public class ProductCartDTO
        {
            public int ProductCardId { get; set; }
         public int ProductId { get; set; }


        [Display(Name = "Category")]
            public string CategoryName { get; set; }

        [Display(Name = "Product")]
        public string ProductName { get; set; }

        [Display(Name = "Price")]
        public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
        }

}
