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
        public async Task<ActionResult> Post(Cart Model)
        {
            var status = await _Cart.Post(Model);
            if (status == true)
            {
                return Ok(new { message = "Added" });
            }
            return Ok(new { message="exist"});
        }

        [HttpGet]
        [Route("GetByUserId/{id}")]
        public async Task<ActionResult<List<vCarts>>> GetByUserId(int Id)
        {
            var cart = await _Cart.GetByUserId(Id);
            if (cart != null)
            {
                return cart;
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetCount/{id}")]
        public async Task<ActionResult<int>> GetCount(int Id)
        {
            return  await _Cart.GetCount(Id);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var status = await _Cart.Delete(Id);
            if (status == true)
            {
                return Ok(new { message = "Deleted" });
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("Update/{id}/{qty}")]
        public async Task<ActionResult> Put(int Id,int Qty)
        {
            var status = await _Cart.Put(Id, Qty);
            if (status == true)
            {
                return Ok(new { message = "Updated" });
            }
            return BadRequest();
        }



        //reminder method for checkout
        [Route("Reminder")]
        [HttpGet]
        public ActionResult Reminder()
        {
          
             _Cart.Reminder();
            return Ok(new {message= "Mail send" });
        }


    }
}