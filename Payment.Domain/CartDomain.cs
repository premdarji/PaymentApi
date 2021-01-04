using Payment.Entity;
using Payment.Entity.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payment.Entity.ViewModels;

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

        public async Task<bool> Delete(int Id)
        {
            var cart = await _context.Cart.FirstOrDefaultAsync(m => m.CartId == Id);
            var test = cart != null ? _context.Cart.Remove(cart) : null;
            //_context.Cart.Remove(cart);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<vCarts>> GetByUserId(int Id)
        {
            return await _context.vCarts.Where(m => m.UserId == Id).ToListAsync();
        }

        public async Task<int> GetCount(int Id)
        {
            var count = await _context.Cart.Where(m => m.UserId.Equals(Id)).CountAsync();
            return count;
        }

        public async Task<bool> Post(Cart Model)
        {
            Model.CreatedOn = DateTime.Now;
            var cart = await _context.Cart.Where(m => m.ProductId == Model.ProductId && m.UserId == Model.UserId).FirstOrDefaultAsync();
            if (cart != null)
            {
                return false;
            }
            else
            {
                _context.Cart.Add(Model);
                var status = await _context.SaveChangesAsync();
                if (status > 0)
                {
                    return true;
                }
                return false;

            }
           
        }

        public async Task<bool> Put(int Id, int Qty)
        {
            var cart = await _context.Cart.FirstOrDefaultAsync(m => m.CartId == Id);
            cart.Quantity = Qty;
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        //reminder method for giving mail after 1 day if item is not checkout from cart

        public void Reminder()
        {
            var todaydate = DateTime.Now;
            todaydate = todaydate.Subtract(TimeSpan.FromDays(1));
             List<Cart> reminders = _context.Cart.Where(x => x.CreatedOn < todaydate).ToList();
           // var reminders = _context.Cart.Where(x => x.CreatedOn < todaydate).FirstOrDefault();
            SendMail(reminders);
            

        }

        public void SendMail(List<Cart> Cart)
        {
            //var email = _context.Users.Where(x => x.UserId == cart.UserId).FirstOrDefault();
            //var produ = _context.Products.Where(x => x.ProductId == cart.ProductId).FirstOrDefault();
            //var message = new Message(email.Email, "Test email", "This email is sent  you to give reminder about product'" + produ.Name + "'which you have order few days ago.");
            //_emailSender.SendEmail(message);
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
        Task<bool> Post(Cart Model);

        Task<List<vCarts>> GetByUserId(int Id);
        Task<int> GetCount(int Id);

        Task<bool> Delete(int Id);

        Task<bool> Put(int Id,int Qty);

        void Reminder();

        void SendMail(List<Cart> Cart);
    }
}
