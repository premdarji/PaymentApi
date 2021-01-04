﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payment.Entity.DbModels
{
   public class vInvoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UserId { get; set; }
        public decimal Total { get; set; }
        public int ProductId { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
