using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Payment.Entity.DbModels
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Column(TypeName ="nvarchar(150)")]
        public string CityName { get; set; }

        public ICollection<User> users { get; set; }
    }
}
