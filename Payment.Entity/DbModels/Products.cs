using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Payment.Entity.DbModels
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }

        [Column(TypeName ="nvarchar(150)")]
        public string Name { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Price { get; set; }

        [Column(TypeName = "int")]
        public int Quantity { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; }

        //[Column(TypeName ="int")]
        //public int Rating { get; set; }

        [Column(TypeName ="nvarchar(max)")]
        public string ImageUrl { get; set; }

        //foreign key
        public int CategoryId { get; set; }

       // public Category Categories { get; set; }

        public ICollection<Cart> Cart { get; set; }

        public ICollection<Wishlist> Wishlist { get; set; }

     //   public ICollection<Order> Order { get; set; }

        public ICollection<DetailOrder> DetailOrders { get; set; }
    }
}
