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
        public async Task<ActionResult> Post(City Model)
        { 

            var status =await city.Post(Model);
            if (status == true)
            {
                return Ok(new { message = "city Added" });
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("UpdateCity")]
        public async Task<ActionResult> Update(City Model)
        {
            if (Model.CityId == 0)
            {
                return BadRequest();
            }
            var status = await city.Put(Model);
            if (status == true)
            {
                return Ok(new { message = "city Updated" });
            }
            return BadRequest();

        }

        [HttpDelete]
        [Route("DeleteCity/{id}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var status =await city.Delete(Id);
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