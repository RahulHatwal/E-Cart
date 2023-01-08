using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using E_Cart_WebApp.Models;

namespace E_Cart_WebApp.DTOs
{

    [Index("CategoryId", Name = "CategoriesProducts")]
    [Index("CategoryId", Name = "CategoryID")]
    [Index("ProductName", Name = "ProductName")]
    [Index("SupplierId", Name = "SupplierID")]
    [Index("SupplierId", Name = "SuppliersProducts")]
    public class ProductUpdateDTO
    {

        [Key]
        public int ProductID { get; set; }
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


        [StringLength(20)]
        public string QuantityPerUnit { get; set; }
        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        [ForeignKey("CategoryID")]
        [InverseProperty("Products")]
        public virtual Category Category { get; set; }
        [ForeignKey("SupplierID")]
        [InverseProperty("Products")]
        public virtual Supplier Supplier { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Order_Detail> Order_Details { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<ProductsCart> ProductsCarts { get; set; }
        //public static implicit operator ProductDTO(ProductDTO v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
