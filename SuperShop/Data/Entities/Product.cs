﻿using SuperShop.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Data.Entity
{
    public class Product: IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage ="The field {0} can contain {1} characters length. ")]
        public string Name { get; set; }


        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }


        [Display(Name = "Image")]
        public string imageUrl { get; set; }

        [Display(Name = "Last Purchase")]
        public DateTime? LastPurchase {  get; set; }


        [Display(Name = "Last")]
        public DateTime? LastSale { get; set; }


        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double Stock {  get; set; }
    }
}
