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
        public CityController(ICityDomain City)
        {
            this.city = City;
                
        }

        [HttpPost]
        [Route("AddCity")]
        public async Task<ActionResult> post(City model)
        { 

            var status =await city.post(model);
            if (status == true)
            {
                return Ok(new { message = "city Added" });
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("UpdateCity")]
        public async Task<ActionResult> update(City model)
        {
            if (model.CityId == 0)
            {
                return BadRequest();
            }
            var status = await city.put(model);
            if (status == true)
            {
                return Ok(new { message = "city Updated" });
            }
            return BadRequest();

        }

        [HttpDelete]
        [Route("DeleteCity/{id}")]
        public async Task<ActionResult> delete(int id)
        {
            var status =await city.delete(id);
            if (status == true)
            {
                return Ok(new { message = "city Deleted" });
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<City>>> getAll()
        {
            var cities =await city.getAll();
            return cities;
        }


    }
}