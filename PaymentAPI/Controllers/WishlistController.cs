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
    public class WishlistController : BaseController
    {
        public WishlistController(IWishlistDomain domain) 
        {
            this._Wishlist = domain;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Wishlist model)
        {
            var status = await _Wishlist.Post(model);
            if (status == true)
            {
                return Ok(new { message = "Added" });
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<List<Wishlist>>> GetAll()
        {
            var wishlist = await _Wishlist.GetAll();
            if (wishlist != null)
            {
                return wishlist;
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Remove/{id}/{userid}")]
        public async Task<ActionResult> Delete(int id,int userid)
        {
            var status = await _Wishlist.Delete(id, userid);
            if (status == true)
            {
                return Ok(new { message = "Deleted" });
            }
            return BadRequest();

        }
    }
}