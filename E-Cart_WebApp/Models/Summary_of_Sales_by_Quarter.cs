﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_Cart_WebApp.Models
{
    [Keyless]
    public partial class Summary_of_Sales_by_Quarter
    {
        [Column(TypeName = "datetime")]
        public DateTime? ShippedDate { get; set; }
        public int OrderID { get; set; }
        [Column(TypeName = "money")]
        public decimal? Subtotal { get; set; }
    }
}