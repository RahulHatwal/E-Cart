﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace E_Cart_WebApp.Models
{
    public partial class CustomerDemographic
    {
        public CustomerDemographic()
        {
            Customers = new HashSet<Customer>();
        }

        [Key]
        [StringLength(10)]
        public string CustomerTypeID { get; set; }
        [Column(TypeName = "ntext")]
        public string CustomerDesc { get; set; }

        [ForeignKey("CustomerTypeID")]
        [InverseProperty("CustomerTypes")]
        public virtual ICollection<Customer> Customers { get; set; }
    }
}