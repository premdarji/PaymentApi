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
        public async Task<bool> delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            var test = product != null ? _context.Products.Remove(product):null;
            //_context.Products.Remove(product);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<ProductListViewModel>> getAll(int id,int pageNumber,int pageSize)
        {
            SqlParameter userId = new SqlParameter("@UserId", id);
            SqlParameter pagenumber = new SqlParameter("@PageNumber", pageNumber);
            SqlParameter pagesize = new SqlParameter("@PageSize", pageSize);

            // Processing.  
            string sqlQuery = "EXEC [dbo].[spProductList] " +
                                "@UserId ,@PageNumber, @PageSize";

            var products = await this._context.Set<ProductListViewModel>().FromSql(sqlQuery, userId, pagenumber, pagesize).ToListAsync();
            return products;
           
        }

        public async Task<List<Products>> getByCategory(int id)
        {
            return await _context.Products.Where(m => m.CategoryId.Equals(id)).ToListAsync();
        }

        public async Task<Products> getById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<bool> post(Products model)
        {
              _context.Products.Add(model);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;

        }

        public async  Task<bool> put(Products model)
        {
            _context.Entry(model).State = EntityState.Modified;
            var status =await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }


        public async Task<List<Products>> getProductsForAdmin()
        {
            return await _context.Products.ToListAsync();
        }


        public List<Products> checkingForSignal()
        {
            return _context.Products.ToList();
        }
    }


    public interface IProductDomain
    {
        Task<List<ProductListViewModel>> getAll(int id,int pageNumber,int pageSize);

        Task<Products> getById(int id);
        Task<bool> post(Products model);
        Task<bool> put(Products model);
        Task<bool> delete(int id);

        Task<List<Products>> getByCategory(int id);

        Task<List<Products>> getProductsForAdmin();

        List<Products> checkingForSignal();
    }

}
