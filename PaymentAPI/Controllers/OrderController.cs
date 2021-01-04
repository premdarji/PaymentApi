using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Domain;
using Payment.Entity.DbModels;
using Payment.Entity.ViewModels;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        public OrderController(IOrderDomain order)
        {
            this._Order = order;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Order model)
        {
            var status = await _Order.Post(model);
            if (status>0)
            {
                return Ok(new { id=status });
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetAll/{id}")]
        public async Task<ActionResult> GetAll(int id)
        {
            var orders = await _Order.GetAll(id);
            if (orders != null)
            {
                return Ok(orders);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Detail")]
        public async Task<ActionResult> StoreDetail(DetailOrder model)
        {
            var status = await _Order.PostDetail(model);
            if(status==true)
            {
                return Ok();
            }
            return BadRequest();
        }



        [HttpPost]
        [Route("Invoice")]
        public async Task<ActionResult> SendInvoiceMail([FromBody]int id)
        {
           await _Order.SendInvoiceMail(id);

            return Ok();
        }

        [HttpGet]
        [Route("OrderById/{id}")]
        public async Task<ActionResult> GetOrderById(int id)
        {
            var order = await _Order.GetOrderById(id);
            if (order != null)
            {
                return Ok(order);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("CancelOrder")]
        public async Task<ActionResult> CancelOrder(CancelOrder model)
        {
            bool status = await _Order.CancelOrder(model);
            if (status == true)
            {
                return Ok(status);
            }
            return BadRequest(status);
        }

       
    }
}