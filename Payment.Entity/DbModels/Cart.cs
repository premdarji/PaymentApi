using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Payment.Entity.DbModels
{
    public class Cart
    {
        [Key]
        public  int CartId { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedOn { get; set; }

        //foreign key to product
        [ForeignKey("Products")]
        public int ProductId { get; set; }

        //foreign key to user
        [ForeignKey("User")]
        public int UserId { get; set; }

   

    }
}
