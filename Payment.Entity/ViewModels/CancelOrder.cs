using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payment.Entity.ViewModels
{
    public class CancelOrder
    {
        [Key]
        public int CancelOrderId { get; set; }

        public int OrderId { get; set; }
        public string Reason { get; set; }
    }
}
