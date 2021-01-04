using Payment.Entity;
using Payment.Entity.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Payment.Domain
{
    public class WishlistDomain : IWishlistDomain
    {
        ApplicationContext _context;
        public WishlistDomain(ApplicationContext Context)
        {
            _context = Context;
                
        }
        public async Task<bool> Delete(int Id,int Userid)
        {
            var wish = await _context.Wishlist.FirstOrDefaultAsync(m => m.ProductId.Equals(Id) && m.UserId.Equals(Userid));
            var test = wish != null ? _context.Wishlist.Remove(wish) : null;
            //_context.Wishlist.Remove(wish);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Wishlist>> GetAll()
        {
            return await _context.Wishlist.ToListAsync();
        }

        public async Task<bool> Post(Wishlist Model)
        {
            var wish = await _context.Wishlist.FirstOrDefaultAsync(m => m.ProductId == Model.ProductId && m.UserId == Model.UserId);
            if (wish == null)
            {
                await _context.Wishlist.AddAsync(Model);
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
    }

    public interface IWishlistDomain
    {
        Task<bool> Post(Wishlist Model);
        Task<bool> Delete(int Id,int Userid);

        Task<List<Wishlist>> GetAll();
    }
}
