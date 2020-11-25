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
    public class CityController : BaseController
    {
        public CityController(ICityDomain city)
        {
            this.city = city;
                
        }

        [HttpPost]
        [Route("AddCity")]
        public async Task<ActionResult> Post(City model)
        { 
            var status =await city.Post(model);
            if (status == true)
            {
                return Ok(new { message = "city Added" });
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("UpdateCity/{id}")]
        public async Task<ActionResult> Update(int id,City model)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var status = await city.Put(id, model);
            if (status == true)
            {
                return Ok(new { message = "city Updated" });
            }
            return BadRequest();

        }

        [HttpDelete]
        [Route("DeleteCity/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var status =await city.Delete(id);
            if (status == true)
            {
                return Ok(new { message = "city Deleted" });
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<City>>> GetAll()
        {
            var cities =await city.GetAll();
            return cities;
        }


    }
}