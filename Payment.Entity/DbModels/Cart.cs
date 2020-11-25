using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payment.Entity.DbModels
{
    public class Cart
    {
        [Key]
        public  int CartId { get; set; }

        public int Quantity { get; set; }

        //foreign key to product
        public int ProductId { get; set; }

        //roreign key to user
        public int UserId { get; set; }

        public Products Product { get; set; }

        public User User { get; set; }

    }
}
