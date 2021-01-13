using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payment.Entity.DbModels
{
    public class Wishlist
    {
        [Key]
        public int wishlistId { get; set; }
        public int ProductId{ get; set; }

        public int UserId { get; set; }

        //public Products Product { get; set; }

        //public User User { get; set; }
    }
}
