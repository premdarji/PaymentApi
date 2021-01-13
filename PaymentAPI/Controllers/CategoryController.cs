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
        public async Task<ActionResult> getAll()
        {
            var cities = await _Category.getAll();
            if (cities != null)
            {
                return Ok(cities);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult> getById(int id)
        {
            var city = await _Category.getById(id);
            if (city != null)
            {
                return Ok(city);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> post(Category model)
        {

            ValidationResult result = validator.Validate(model);

            if (result.IsValid == false)
            {
                return BadRequest(new { result.Errors });
            }

            var status = await _Category.post(model);
            if (status == true)
            {
                return Ok(new { message = "Category Added" });
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult> update(Category model)
        {
            ValidationResult result = validator.Validate(model);

            if (result.IsValid == false)
            {
                return BadRequest(new { result.Errors });
            }



            var status =await  _Category.put(model);
            if (status == true)
            {
                return Ok(new { message = "Category Updated" });
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> delete(int id)
        {
            var status =  await _Category.delete(id);
            if (status == true)
            {
                return Ok(new { message = "Category Deleted" });
            }
            return BadRequest();

        }

    }
}