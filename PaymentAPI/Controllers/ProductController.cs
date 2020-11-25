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
    public class ProductController : BaseController
    {

        ProductValidator validator = new ProductValidator();

        public ProductController(IProductDomain product)
        {
            this._Product = product;
        }

        [HttpGet]
        [Route("GetAll/{id}/{pagenumber}/{pagesize}")]
        public async Task<ActionResult> GetAll(int id,int pagenumber,int pagesize)
        {
            var products =await _Product.GetAll(id,pagenumber,pagesize);
            if (products.Count> 0)
            {
                return Ok(products);
            }
            return Ok(new { message = "nodata" });
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var product =await _Product.GetById(id);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();

        }

        [HttpGet]
        [Route("GetByCategory/{id}")]
        public async Task<ActionResult> GetByCategory(int id)
        {
            var product = await _Product.GetByCategory(id);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();

        }


        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> Post(Products model)
        {

            ValidationResult result = validator.Validate(model);

            if (result.IsValid == false)
            {
                return BadRequest(new { result.Errors });
            }

            var status = await _Product.Post(model);
            if (status==true)
            {
                return Ok(new { message = "Product Added" });
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("Update/{id}")]
        public async Task<ActionResult> Put(int id,Products model)
        {

            ValidationResult result = validator.Validate(model);

            if (result.IsValid == false)
            {
                return BadRequest(new { result.Errors });
            }

            var status = await _Product.Put(id,model);
            if (status == true)
            {
                return Ok(new { message = "Product Updated" });
            }
            return BadRequest();

        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var status = await _Product.Delete(id);
            if (status == true)
            {
                return Ok(new { message = "Product Deleted" });
            }
            return BadRequest();

        }
    }
}