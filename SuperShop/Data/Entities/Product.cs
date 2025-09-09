using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SuperShop.Data.Entities;

namespace SuperShop.Data.Entity
{
    public class Product: IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage ="The field {0} can contain {1} characters length. ")]
        public string Name { get; set; }


        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Column(TypeName = "decimal(18,2)")]  
        public decimal Price { get; set; }


        [Display(Name = "Image")]
        public string ImageId { get; set; }

        [Display(Name = "Last Purchase")]
        public DateTime? LastPurchase {  get; set; }


        [Display(Name = "Last Sale")]
        public DateTime? LastSale { get; set; }


        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double Stock {  get; set; }

        public User User { get; set; }


        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageId))
                {
                    return null;
                }
                return $"https://localhost:44311{ImageId.Substring(1)}";
            }
            
        }

       
    }
}
