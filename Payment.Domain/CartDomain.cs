using Payment.Entity;
using Payment.Entity.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payment.Entity.ViewModels;
using System.Data.SqlClient;

namespace Payment.Domain
{
    public class CartDomain : ICartDomain
    {
        ApplicationContext _context;
        private IEmailSender _emailSender;


        public CartDomain(ApplicationContext Context)
        {
            _context = Context;
        }

        public CartDomain(ApplicationContext Context,IEmailSender Sender)
        {
            _context = Context;
            _emailSender = Sender;
        }

        public async Task<bool> delete(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            var test = cart != null ? _context.Cart.Remove(cart) : null;
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<vCarts>> getByUserId(int id)
        {

            SqlParameter userId = new SqlParameter("@userId", id);

            // Processing.  
            string sqlQuery = "EXEC [dbo].[CartItems] @userId";

            var cartItems = await this._context.Set<vCarts>().FromSql(sqlQuery, userId).ToListAsync();
            return cartItems;     
        }

        public async Task<int> getCount(int id)
        {
            var count = await _context.Cart.Where(m => m.UserId.Equals(id)).CountAsync();
            return count;
        }

        public async Task<bool> post(Cart model)
        {
            model.CreatedOn = DateTime.Now;
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

        public async Task<bool> removeFromCart(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            cart.Quantity -=1 ;
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> addToCart(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            cart.Quantity += 1;
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        //reminder method for giving mail after 1 day if item is not checkout from cart

        public void reminder()
        {
            var todaydate = DateTime.Now;
            todaydate = todaydate.Subtract(TimeSpan.FromDays(1));
             List<Cart> reminders = _context.Cart.Where(x => x.CreatedOn < todaydate).ToList();
           // var reminders = _context.Cart.Where(x => x.CreatedOn < todaydate).FirstOrDefault();
            sendMail(reminders);
            

        }

        public void sendMail(List<Cart> Cart)
        {
    
            foreach (var item in Cart)
            {
                var email = _context.Users.Where(x => x.UserId == item.UserId).FirstOrDefault();
                var produ = _context.Products.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                var message = new Message(email.Email, "Test email", "This email is sent  you to give reminder about product'" + produ.Name + "'which you have order few days ago.");
                _emailSender.SendEmail(message);

            }
        }

    }

    public interface ICartDomain
    {
        Task<bool> post(Cart model);

        Task<List<vCarts>> getByUserId(int id);
        Task<int> getCount(int id);

        Task<bool> delete(int id);

        Task<bool> removeFromCart(int Id);

        Task<bool> addToCart(int id);

        void reminder();

        void sendMail(List<Cart> cart);
    }
}
