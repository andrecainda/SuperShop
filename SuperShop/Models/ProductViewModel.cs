using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using SuperShop.Data.Entity;


namespace SuperShop.Models
{
    public class ProductViewModel : Product
    {
        [Display(Name = "Name")]
        public IFormFile ImageFile { get; set; }

    }
}
