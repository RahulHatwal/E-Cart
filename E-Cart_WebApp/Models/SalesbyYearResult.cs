﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Cart_WebApp.Models
{
    public partial class SalesbyYearResult
    {
        public DateTime? ShippedDate { get; set; }
        public int OrderID { get; set; }
        public decimal? Subtotal { get; set; }
        public string Year { get; set; }
    }
}
