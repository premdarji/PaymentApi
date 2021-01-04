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
        public OrderDomain(ApplicationContext Context,IEmailSender EmailSender)
        {
            _context = Context;
            _emailSender = EmailSender;
        }

        public async Task<List<vOrder>> GetAll(int Id)
        {
            return await _context.vOrder.Where(m => m.UserId == Id).ToListAsync();
        }

        public async Task<vOrder> GetOrderById(int Id)
        {
            return await _context.vOrder.Where(x => x.DetailOrderId == Id).FirstOrDefaultAsync();
        }

        public async Task<int> Post(Order Model)
        {
            Model.CreatedOn = DateTime.Now;
            await _context.Order.AddAsync(Model);
            var status = await _context.SaveChangesAsync();
            int id = Model.OrderId;
            int userid = Model.UserId;

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

        public async Task<bool> PostDetail(DetailOrder Model)
        {
            
            _context.DetailOrders.Add(Model);
            _context.SaveChanges();
            var Product = _context.Products.FirstOrDefault(m => m.ProductId == Model.ProductId);
            Product.Quantity = Product.Quantity - Model.Quantity;
            var status =  await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

    
        public async Task SendInvoiceMail(int Id)
        {
            var order = await _context.Order.Where(x => x.OrderId == Id).FirstOrDefaultAsync();
            var profile = await _context.Users.Where(x => x.UserId == order.UserId).FirstOrDefaultAsync();
            var invoiceItems = await _context.vOrder.Where(x => x.OrderId == Id).ToListAsync();
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

        public async Task<bool> CancelOrder(CancelOrder Model)
        {
            var order =await  _context.DetailOrders.Where(x => x.DetailOrderId == Model.OrderId).FirstOrDefaultAsync();
            if (order != null)
            {
                var product = await _context.Products.Where(x => x.ProductId == order.ProductId).FirstOrDefaultAsync();
                 _context.DetailOrders.Remove(order);

                _context.CancelOrder.Add(Model);
                product.Quantity = product.Quantity + order.Quantity;
                await SendCancellationMail(Model.OrderId);
                var status = await _context.SaveChangesAsync();
                if (status > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task SendCancellationMail(int Id)
        {
            var order = await _context.vOrder.Where(x => x.DetailOrderId == Id).FirstOrDefaultAsync();
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
        Task<int> Post(Order Model);

        Task<List<vOrder>> GetAll(int Id);

        Task<bool> PostDetail(DetailOrder  Model);

      

        Task SendInvoiceMail(int Id);

        Task<vOrder> GetOrderById(int Id);

        Task<bool> CancelOrder(CancelOrder Model);

        Task SendCancellationMail(int Id);



    }
}
