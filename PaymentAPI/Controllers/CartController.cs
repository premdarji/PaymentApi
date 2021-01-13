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
    
    public class CartController : BaseController
    {        
        public CartController(ICartDomain Cart)
        { 
            this._Cart = Cart;
        }

        [HttpPost]
        public async Task<ActionResult> post(Cart model)
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
        public async Task<ActionResult<List<vCarts>>> getByUserId(int id)
        {
            var cart = await _Cart.getByUserId(id);
            if (cart != null)
            {
                return cart;
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetCount/{id}")]
        public async Task<ActionResult<int>> getCount(int id)
        {
            return  await _Cart.getCount(id);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> delete(int id)
        {
            var status = await _Cart.delete(id);
            if (status == true)
            {
                return Ok(new { message = "Deleted" });
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("Remove")]
        public async Task<ActionResult> removeFromCart([FromBody]int id)
        {
            var status = await _Cart.removeFromCart(id);
            if (status == true)
            {
                return Ok(new { message = "Updated" });
            }
            return BadRequest();
        }


        [HttpPut]
        [Route("AddToCart")]
        public async Task<ActionResult> addToCart([FromBody]int id)
        {
            var status = await _Cart.addToCart(id);
            if (status == true)
            {
                return Ok(new { message = "Updated" });
            }
            return BadRequest();
        }


        //reminder method for checkout
        [Route("Reminder")]
        [HttpGet]
        public ActionResult reminder()
        {
             _Cart.reminder();
            return Ok(new {message= "Mail send" });
        }


    }
}