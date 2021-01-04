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
        public WishlistController(IWishlistDomain Domain) 
        {
            this._Wishlist = Domain;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Wishlist Model)
        {
            var status = await _Wishlist.Post(Model);
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
        public async Task<ActionResult> Delete(int Id,int Userid)
        {
            var status = await _Wishlist.Delete(Id, Userid);
            if (status == true)
            {
                return Ok(new { message = "Deleted" });
            }
            return BadRequest();

        }
    }
}