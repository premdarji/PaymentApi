using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payment.Entity.DbModels
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        public int OrderId { get; set; }
        public DateTime CreatedOn { get; set; }
      
    }
}
