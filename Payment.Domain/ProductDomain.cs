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
        public async Task<bool> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId.Equals(id));
            _context.Products.Remove(product);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<ProductListViewModel>> GetAll(int id,int pagenumber,int pagesize)
        {
            SqlParameter usernameParam = new SqlParameter("@UserId", id);
            SqlParameter Pagenumber = new SqlParameter("@PageNumber", pagenumber);
            SqlParameter Pagesize = new SqlParameter("@PageSize", pagesize);

            // Processing.  
            string sqlQuery = "EXEC [dbo].[spProductList] " +
                                "@UserId ,@PageNumber, @PageSize";

            var products =await  this._context.Set<ProductListViewModel>().FromSql(sqlQuery, usernameParam,Pagenumber,Pagesize).ToListAsync();
            return products;
        }

        public async Task<List<Products>> GetByCategory(int id)
        {
            return await _context.Products.Where(m => m.CategoryId.Equals(id)).ToListAsync();
        }

        public async Task<Products> GetById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(m => m.ProductId.Equals(id));
        }

        public async Task<bool> Post(Products model)
        {
          
             await _context.Products.AddAsync(model);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;

        }

        public async  Task<bool> Put(int id, Products model)
        {
            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId.Equals(id));
            product.Price = model.Price;
            //            _context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            var status =await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }
    }


    public interface IProductDomain
    {
        Task<List<ProductListViewModel>> GetAll(int id,int pagenumber,int pagesize);

        Task<Products> GetById(int id);
        Task<bool> Post(Products model);
        Task<bool> Put(int id, Products model);
        Task<bool> Delete(int id);

        Task<List<Products>> GetByCategory(int id);
    }

}
