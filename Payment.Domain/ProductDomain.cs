using Payment.Entity;
using Payment.Entity.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Payment.Entity.ViewModels;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Payment.Domain
{
    public class ProductDomain : IProductDomain
    {
        ApplicationContext _context;

        public ProductDomain(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
           // var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId.Equals(Id));
            var test = product != null ? _context.Products.Remove(product):null;
            //_context.Products.Remove(product);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<ProductListViewModel>> GetAll(int Id,int Pagenumber,int Pagesize)
        {
            SqlParameter usernameParam = new SqlParameter("@UserId", Id);
            SqlParameter pagenumber = new SqlParameter("@PageNumber", Pagenumber);
            SqlParameter pagesize = new SqlParameter("@PageSize", Pagesize);

            // Processing.  
            string sqlQuery = "EXEC [dbo].[spProductList] " +
                                "@UserId ,@PageNumber, @PageSize";

            var products = await this._context.Set<ProductListViewModel>().FromSql(sqlQuery, usernameParam, pagenumber, pagesize).ToListAsync();
            return products;
           
        }

        public async Task<List<Products>> GetByCategory(int Id)
        {
            return await _context.Products.Where(m => m.CategoryId.Equals(Id)).ToListAsync();
        }

        public async Task<Products> GetById(int Id)
        {
            return await _context.Products.FirstOrDefaultAsync(m => m.ProductId.Equals(Id));
        }

        public async Task<bool> Post(Products Model)
        {
              _context.Products.Add(Model);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;

        }

        public async  Task<bool> Put(Products Model)
        {
            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId.Equals(Model.ProductId));
            product.Price = Model.Price;
            product.Description = Model.Description;
            product.Name = Model.Name;
            product.Quantity = Model.Quantity;
            product.CategoryId = Model.CategoryId;
            product.ImageUrl = Model.ImageUrl;
         
            var status =await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }


        public async Task<List<Products>> GetProductsForAdmin()
        {
            return await _context.Products.ToListAsync();
        }


        public List<Products> CheckingForSignal()
        {
            return _context.Products.ToList();
        }
    }


    public interface IProductDomain
    {
        Task<List<ProductListViewModel>> GetAll(int Id,int Pagenumber,int Pagesize);

        Task<Products> GetById(int Id);
        Task<bool> Post(Products Model);
        Task<bool> Put(Products Model);
        Task<bool> Delete(int Id);

        Task<List<Products>> GetByCategory(int Id);

        Task<List<Products>> GetProductsForAdmin();

        List<Products> CheckingForSignal();
    }

}
