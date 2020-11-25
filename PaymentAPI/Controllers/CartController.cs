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
    
    public class CartController : BaseController
    {
        public CartController(ICartDomain cart)
        {
            this._Cart = cart;
        }

        [HttpPost]
        
        public async Task<ActionResult> Post(Cart model)
        {
            var status = await _Cart.post(model);
            if (status == true)
            {
                return Ok(new { message = "Added" });
            }
            return Ok(new { message="exist"});
        }

        [HttpGet]
        [Route("GetByUserId/{id}")]
        public async Task<ActionResult<List<vCarts>>> GetByUserId(int id)
        {
            var cart = await _Cart.GetByUserId(id);
            if (cart != null)
            {
                return cart;
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetCount/{id}")]
        public async Task<ActionResult<int>> GetCount(int id)
        {
            return  await _Cart.GetCount(id);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var status = await _Cart.Delete(id);
            if (status == true)
            {
                return Ok(new { message = "Deleted" });
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("Update/{id}/{qty}")]
        public async Task<ActionResult> Put(int id,int qty)
        {
            var status = await _Cart.Put(id, qty);
            if (status == true)
            {
                return Ok(new { message = "Updated" });
            }
            return BadRequest();
        }

    }
}