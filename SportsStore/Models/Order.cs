using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models
{
    public class Order
    {
        [BindNever]
        public int OrderID { get; set; }
        [BindNever]
        public ICollection<CartLine> Lines { get; set; } = new List<CartLine>();

        [Required(ErrorMessage ="Please enter a name")]
        public string? Name { get; set; }

        [Required(ErrorMessage ="Please enter the first address line")]
        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? line3 { get; set; }

        [Required(ErrorMessage ="please enter a city")]
        public string? City { get; set; }

        [Required(ErrorMessage = "please enter a state")]
        public string? State { get; set; }

        public string? Zip { get; set; }

        [Required(ErrorMessage = "please enter a country name")]
        public string? Country { get; set; }

        public bool GiftWrap { get; set; }
    }
}
