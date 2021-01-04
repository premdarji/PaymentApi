using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Core.Filters;
using Payment.Domain;
using Payment.Entity.DbModels;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {

        CategoryValidator validator = new CategoryValidator();

        public CategoryController(ICategoryDomain Category)
        {
            this._Category = Category;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            var cities = await _Category.GetAll();
            if (cities != null)
            {
                return Ok(cities);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult> GetById(int Id)
        {
            var city = await _Category.GetById(Id);
            if (city != null)
            {
                return Ok(city);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> Post(Category Model)
        {

            ValidationResult result = validator.Validate(Model);

            if (result.IsValid == false)
            {
                return BadRequest(new { result.Errors });
            }

            var status = await _Category.Post(Model);
            if (status == true)
            {
                return Ok(new { message = "Category Added" });
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult> Update(Category Model)
        {
            ValidationResult result = validator.Validate(Model);

            if (result.IsValid == false)
            {
                return BadRequest(new { result.Errors });
            }



            var status =await  _Category.Put(Model);
            if (status == true)
            {
                return Ok(new { message = "Category Updated" });
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var status =  await _Category.Delete(Id);
            if (status == true)
            {
                return Ok(new { message = "Category Deleted" });
            }
            return BadRequest();

        }

    }
}