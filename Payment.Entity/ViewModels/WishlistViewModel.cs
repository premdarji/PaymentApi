using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payment.Entity.ViewModels
{
    public class WishlistViewModel
    {
        [Key]
        public int wishlistId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description  { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}
