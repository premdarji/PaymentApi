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
    public class OrderDomain : IOrderDomain
    {
        ApplicationContext _context;
        public OrderDomain(ApplicationContext context)
        {
            _context = context;       
        }

        public async Task<List<vOrder>> GetAll(int id)
        {
            return await _context.vOrder.Where(m => m.UserId == id).ToListAsync();
        }

        public async Task<int> Post(Order model)
        {
            model.CreatedOn = DateTime.Now;
            await _context.Order.AddAsync(model);
            var status = await _context.SaveChangesAsync();
            int id = model.OrderId;
            if (status > 0)
            {
                return id;
            }
            return 0;
        }

        public async Task<bool> PostDetail(DetailOrder model)
        {
            
            _context.DetailOrders.Add(model);
            var Product = _context.Products.FirstOrDefault(m => m.ProductId == model.ProductId);
            Product.Quantity = Product.Quantity - model.Quantity;
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }
    }

    public interface IOrderDomain
    {
        Task<int> Post(Order model);

        Task<List<vOrder>> GetAll(int id);

        Task<bool> PostDetail(DetailOrder model);
    }
}
