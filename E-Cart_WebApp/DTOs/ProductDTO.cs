using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Cart_WebApp.DTOs
{
    //[Index("CategoryId", Name = "CategoriesProducts")]
    //[Index("CategoryId", Name = "CategoryID")]
    //[Index("ProductName", Name = "ProductName")]
    //[Index("SupplierId", Name = "SupplierID")]
    //[Index("SupplierId", Name = "SuppliersProducts")]
    public class ProductDTO
    {

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }
        [Column("SupplierID")]
        public int? SupplierId { get; set; }
        [Column("CategoryID")]
        public int? CategoryId { get; set; }

        public string SelectedCategory { get; set; }
        public string SelectedSupplier { get; set; }
        public List<SelectListItem> CategoriesSelectList { get; set; }
        public List<SelectListItem> SupplierSelectList { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Quantity should be positive integer")]
        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        // This regular expression allows for any positive decimal number, including numbers with decimal places.
        [RegularExpression(@"^[+]?([0-9]+(?:[\.][0-9]*)?|\.[0-9]+)$", ErrorMessage = "Invalid Unit Price")]
        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }


        // Any number that doesn't starts with zero
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Invalid ReOrder Level")]
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        //public static implicit operator ProductDTO(ProductDTO v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
