using System;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class DeliveryViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Delivery date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }
    }
}
