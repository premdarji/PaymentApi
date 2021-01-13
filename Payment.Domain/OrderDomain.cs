using Payment.Entity;
using Payment.Entity.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payment.Entity.ViewModels;
using Microsoft.AspNetCore.Http.Internal;

using System.IO;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Payment.Domain
{
    public class OrderDomain : IOrderDomain
    {
        ApplicationContext _context;
        IEmailSender _emailSender;
        public OrderDomain(ApplicationContext context,IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<List<vOrder>> getAll(int id)
        {
            return await _context.vOrder.Where(m => m.UserId == id && m.IsDeleted==false).ToListAsync();
        }

        public async Task<vOrder> getOrderById(int id)
        {
            return await _context.vOrder.Where(x => x.DetailOrderId == id).FirstOrDefaultAsync();
        }

        public async Task<int> post(Order model)
        {
            if (model.PaymentId == "wallet")
            {
                var user = await _context.Users.FindAsync(model.UserId);
                user.WalletAmt -= model.Amount;
                //await _context.SaveChangesAsync();
            }
            model.CreatedOn = DateTime.Now;
            await _context.Order.AddAsync(model);
            var status = await _context.SaveChangesAsync();
            int id = model.OrderId;
            int userid = model.UserId;

            //invoice
            Invoice invoice = new Invoice()
            {
                CreatedOn = DateTime.Now,
                OrderId = id

            };
            await _context.Invoice.AddAsync(invoice);
            await _context.SaveChangesAsync();

            if (status > 0)
            {
                return id;
            }
            return 0;
        }

        public async Task<bool> postDetail(DetailOrder model)
        {
            
            _context.DetailOrders.Add(model);
            _context.SaveChanges();
            var Product = _context.Products.FirstOrDefault(m => m.ProductId == model.ProductId);
            Product.Quantity = Product.Quantity - model.Quantity;
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

    
        public async Task sendInvoiceMail(int id)
        {
            var order = await _context.Order.Where(x => x.OrderId == id).FirstOrDefaultAsync();
            var profile = await _context.Users.Where(x => x.UserId == order.UserId).FirstOrDefaultAsync();
            var invoiceItems = await _context.vOrder.Where(x => x.OrderId == id).ToListAsync();
            string details=null;
            foreach(var item in invoiceItems)
            {
              var temp= "<tr><td>" + item.Name + "</td><td>" + item.Price + "</td><td>" + item.Quantity + "</td><td>" + item.Amount + "</td></tr>";
                details = details + temp;
            }
            
            var data = "<div class='container'><table style='border-block-width: 4px;font-size:medium;border-style:dashed;'>" +
                        "<tr><th>Name</th><th>Price</th><th>Quantity</th><th>Amount</th></tr>" +
                        details+
                    "</table>" +
                    "</div>";

            //var message = new Message(profile.Email, "Invoice Details", "This email is sent you to give invoice");
            //await _emailSender.ConfirmationEmail(message,Id);
            var message = new Message(profile.Email, "Order Confirmation", data);
            _emailSender.SendEmail(message);
        }

        public async Task<bool> cancelOrder(CancelOrder model)
        {
            var order =await  _context.DetailOrders.Where(x => x.DetailOrderId == model.OrderId).FirstOrDefaultAsync();
            if (order != null)
            {
                var product = await _context.Products.Where(x => x.ProductId == order.ProductId).FirstOrDefaultAsync();
                // _context.DetailOrders.Remove(order);
                order.IsDeleted = true;
                _context.CancelOrder.Add(model);
                product.Quantity = product.Quantity + order.Quantity;
                await sendCancellationMail(model.OrderId);
                var status = await _context.SaveChangesAsync();
                if (status > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task sendCancellationMail(int id)
        {
            var order = await _context.vOrder.Where(x => x.DetailOrderId == id).FirstOrDefaultAsync();
            var profile = await _context.Users.Where(x => x.UserId == order.UserId).FirstOrDefaultAsync();

            var data = "<div class='container'>" +
                            "<table style='border-block-width: 4px;font-size:medium;border-style:dashed;'>" +
                                "<tr style='border-style:ridge;'>" +
                                "<th>OrderId </th><th>Name</th><th>Price</th><th>Quantity</th><th>Amount</th>" +
                                 "</tr>" +
                                 "<tr>"+
                                 "<td>"+order.DetailOrderId+"</td>"+
                                 "<td>"+order.Name+"</td>"+
                                 "<td>" + order.Price + "</td>" +
                                  "<td>" + order.Quantity + "</td>" +
                                 "<td>" + order.Amount + "</td>" +
                                 "</tr>" +
                             "</table>" +
                             "<b>The Amount you get as refund is:-"+order.Amount+"</b>"+
                        "</div>";
            

            var message = new Message(profile.Email, "Cancellation Confirmation", data);
            _emailSender.SendEmail(message);

        }








    }

    public interface IOrderDomain
    {
        Task<int> post(Order model);

        Task<List<vOrder>> getAll(int id);

        Task<bool> postDetail(DetailOrder  model);

      

        Task sendInvoiceMail(int id);

        Task<vOrder> getOrderById(int id);

        Task<bool> cancelOrder(CancelOrder model);

        Task sendCancellationMail(int id);



    }
}
