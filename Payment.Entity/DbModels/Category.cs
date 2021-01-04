using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Payment.Entity.DbModels
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Column(TypeName ="nvarchar(150)")]
        public string Name { get; set; }

        public ICollection<Products> products { get; set; } 
    }
}
