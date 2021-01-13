using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payment.Entity.DbModels
{
    public class vOrder
    {
        [Key]
        public int DetailOrderId { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UserId { get; set; }

        public int OrderId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
