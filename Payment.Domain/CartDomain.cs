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
    public class CartDomain : ICartDomain
    {
        ApplicationContext _context;
        public CartDomain(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int id)
        {
            var cart = await _context.Cart.FirstOrDefaultAsync(m => m.CartId == id);
            _context.Cart.Remove(cart);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<vCarts>> GetByUserId(int id)
        {
            return await _context.vCarts.Where(m => m.UserId == id).ToListAsync();
        }

        public async Task<int> GetCount(int id)
        {
            var count = await _context.Cart.Where(m => m.UserId.Equals(id)).CountAsync();
            return count;
        }

        public async Task<bool> post(Cart model)
        {
            var cart = await _context.Cart.Where(m => m.ProductId == model.ProductId && m.UserId == model.UserId).FirstOrDefaultAsync();
            if (cart != null)
            {
                return false;
            }
            else
            {
                _context.Cart.Add(model);
                var status = await _context.SaveChangesAsync();
                if (status > 0)
                {
                    return true;
                }
                return false;

            }
           
        }

        public async Task<bool> Put(int id, int qty)
        {
            var cart = await _context.Cart.FirstOrDefaultAsync(m => m.CartId == id);
            cart.Quantity = qty;
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }
    }

    public interface ICartDomain
    {
        Task<bool> post(Cart model);

        Task<List<vCarts>> GetByUserId(int id);
        Task<int> GetCount(int id);

        Task<bool> Delete(int id);

        Task<bool> Put(int id,int qty);
    }
}
