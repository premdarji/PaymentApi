using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Payment.Entity.DbModels
{
    public class vCarts
    {
        public int Quantity { get; set; }
        [Key]
        public int CartId { get; set; }
       // public int UserId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        //public int ProductId { get; set; }

        public int Stock { get; set; }
    }
}
