using Microsoft.EntityFrameworkCore;
using Payment.Entity.DbModels;
using Payment.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Entity
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<Products> Products { get; set; }

        public DbSet<Category> Catergory { get; set; }

        public DbSet<Cart> Cart { get; set; }

        public DbSet<Wishlist> Wishlist { get; set; }

        public DbSet<Order> Order { get; set; }

       public DbSet<vCarts> vCarts { get; set; }

       public DbSet<ProductListViewModel> ProductListViewModel { get; set; }

       public DbSet<vOrder> vOrder { get; set; } 

        
       public DbSet<DetailOrder> DetailOrders { get; set; } 


    }
}
