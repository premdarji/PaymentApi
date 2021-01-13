using Payment.Entity;
using Payment.Entity.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Payment.Entity.ViewModels;

namespace Payment.Domain
{
    public class WishlistDomain : IWishlistDomain
    {
        ApplicationContext _context;
        public WishlistDomain(ApplicationContext context)
        {
            _context = context;
                
        }
        public async Task<bool> remove(int id,int userId)
        {
            var wish = await _context.Wishlist.FirstOrDefaultAsync(m => m.ProductId.Equals(id) && m.UserId.Equals(userId));
            var test = wish != null ? _context.Wishlist.Remove(wish) : null;
            //_context.Wishlist.Remove(wish);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<WishlistViewModel>> getAll(int id)
        {

            SqlParameter userId = new SqlParameter("@userId", id);

            // Processing.  
            string sqlQuery = "EXEC [dbo].[wishlistItems] @userId";

            var wishListItems = await this._context.Set<WishlistViewModel>().FromSql(sqlQuery, userId).ToListAsync();
            return wishListItems;

            //return await _context.Wishlist.ToListAsync();
        }

        public async Task<bool> post(Wishlist model)
        {
            var wish = await _context.Wishlist.FirstOrDefaultAsync(m => m.ProductId == model.ProductId && m.UserId == model.UserId);
            if (wish == null)
            {
                await _context.Wishlist.AddAsync(model);
                var status =await _context.SaveChangesAsync();
                if (status > 0)
                {
                    return true;
                }
                return false;

            }
            else
            {
                return false;
            }
           
        }

        public async Task<bool> delete(int id)
        {
            var wish = await _context.Wishlist.FindAsync(id);
            _context.Wishlist.Remove(wish);
            var status=await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }
    }

    public interface IWishlistDomain
    {
        Task<bool> post(Wishlist model);
        Task<bool> remove(int id,int userId);

        Task<bool> delete(int id);

        Task<List<WishlistViewModel>> getAll(int id);
    }
}
