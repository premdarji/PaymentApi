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
        public async Task<ActionResult> post(Wishlist model)
        {
            var status = await _Wishlist.post(model);
            if (status == true)
            {
                return Ok(new { message = "Added" });
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<List<Wishlist>>> getAll()
        {
            var wishlist = await _Wishlist.getAll();
            if (wishlist != null)
            {
                return wishlist;
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Remove/{id}/{userid}")]
        public async Task<ActionResult> delete(int id,int userId)
        {
            var status = await _Wishlist.delete(id, userId);
            if (status == true)
            {
                return Ok(new { message = "Deleted" });
            }
            return BadRequest();

        }
    }
}