using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payment.Entity.ViewModels
{
    public class ProductListViewModel
    {
        [Key]
        public int ProductId { get; set; }


        public string Name { get; set; }


        public decimal Price { get; set; }


        public int Quantity { get; set; }


        public string Description { get; set; }


       //public int Rating { get; set; }


        public string ImageUrl { get; set; }

        //foreign key
        public int CategoryId { get; set; }

        public bool IsWishListItem { get; set; }
    }

}
