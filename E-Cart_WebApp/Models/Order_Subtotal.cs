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
    public partial class Order_Subtotal
    {
        public int OrderID { get; set; }
        [Column(TypeName = "money")]
        public decimal? Subtotal { get; set; }
    }
}