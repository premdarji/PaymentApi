using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payment.Entity.DbModels
{
    public class DetailOrder
    {
        [Key]
        public int DetailOrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }

        public Products Product { get; set; }
        public Order Order { get; set; }
    }
}
