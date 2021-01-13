using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Payment.Entity.DbModels
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Column(TypeName ="nvarchar(200)")]
        public string PaymentId { get; set; }


        [Column(TypeName ="decimal")]
        public decimal Amount { get; set; }

        [Column(TypeName ="date")]
        public DateTime CreatedOn { get; set; }

        //foreign key
        [Column(TypeName ="int")]
        public int UserId { get; set; }



        //public User User { get; set; }


        public ICollection<DetailOrder> DetailOrders { get; set; }
    }
}
