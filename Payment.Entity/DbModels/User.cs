using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Payment.Entity.DbModels
{
    public class User
    {
        [Key]
        public int UserId{ get; set; }

        [Column(TypeName ="varchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }


        [Column(TypeName = "nvarchar(10)")]
        public string Phone { get; set; }


        [Column(TypeName = "nvarchar(max)")]
        public string Address { get; set; }


        [Column(TypeName = "nvarchar(max)")]
        public string Password { get; set; }


        //foreign key to city table
        public int CityId { get; set; }


        public City city { get; set; }

        public ICollection<Cart> Cart { get; set; }

        public ICollection<Wishlist> Wishlist { get; set; }

        public ICollection<Order> Order { get; set; }
    }
}
