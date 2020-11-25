using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Domain;
using Payment.Entity.DbModels;

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
    }
}