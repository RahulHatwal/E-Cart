// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_Cart_WebAPI.Models
{
    [Table("ProductsCart")]
    public partial class ProductsCart
    {
        [Key]
        [Column("ProductCardID")]
        public int ProductCardId { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public int? CartQuantity { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("ProductsCarts")]
        public virtual Product Product { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("ProductsCarts")]
        public virtual UserMaster User { get; set; }
    }
}