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
        public async Task<ActionResult> post(Order model)
        {
            var status = await _Order.post(model);
            if (status>0)
            {
                return Ok(new { id=status });
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetAll/{id}")]
        public async Task<ActionResult> getAll(int id)
        {
            var orders = await _Order.getAll(id);
            if (orders != null)
            {
                return Ok(orders);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Detail")]
        public async Task<ActionResult> storeDetail(DetailOrder model)
        {
            var status = await _Order.postDetail(model);
            if(status==true)
            {
                return Ok();
            }
            return BadRequest();
        }



        [HttpPost]
        [Route("Invoice")]
        public async Task<ActionResult> sendInvoiceMail([FromBody]int id)
        {
            await _Order.sendInvoiceMail(id);

            return Ok(true);
        }

        [HttpGet]
        [Route("OrderById/{id}")]
        public async Task<ActionResult> getOrderById(int id)
        {
            var order = await _Order.getOrderById(id);
            if (order != null)
            {
                return Ok(order);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("CancelOrder")]
        public async Task<ActionResult> cancelOrder(CancelOrder model)
        {
            bool status = await _Order.cancelOrder(model);
            if (status == true)
            {
                return Ok(status);
            }
            return BadRequest(status);
        }

       
    }
}