using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Payment.Core.Filters;
using Payment.Domain;
using Payment.Entity.DbModels;
using Payment.Entity.ViewModels;
using PaymentAPI.HubConfig;
using PaymentAPI.TimerFeature;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {

        ProductValidator validator = new ProductValidator();

        private IHubContext<DataHub> _hub;

        public ProductController(IProductDomain product, IHubContext<DataHub> hub)
        {
            this._Product = product;
            _hub = hub;
        }

        [HttpGet]
        [Route("GetAll/{id}/{pagenumber}/{pagesize}")]
        public async Task<ActionResult> getAll(int id,int pageNumber,int pageSize)
        {
            pageSize=pageNumber==5?pageSize=20:pageSize=6;
            var products =await _Product.getAll(id,pageNumber,pageSize);
            if (products.Count> 0)
            {
                return Ok(products);
            }
            return Ok(new { message = "nodata" });
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult> getById(int id)
        {
            var product =await _Product.getById(id);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();

        }

        [HttpGet]
        [Route("GetByCategory/{id}")]
        public async Task<ActionResult> getByCategory(int id)
        {
            var product = await _Product.getByCategory(id);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();

        }


        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> post(Products model)
        {

            ValidationResult result = validator.Validate(model);

            if (result.IsValid == false)
            {
                return BadRequest(new { result.Errors });
            }

            var status = await _Product.post(model);
            if (status==true)
            {
                return Ok(new { message = "Product Added" });
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult> put(Products model)
        {

            ValidationResult result = validator.Validate(model);

            if (result.IsValid == false)
            {
                return BadRequest(new { result.Errors });
            }

            var status = await _Product.put(model);
            if (status == true)
            {
                return Ok(new { message = "Product Updated" });
            }
            return BadRequest();

        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> delete(int id)
        {
            var status = await _Product.delete(id);
            if (status == true)
            {
                return Ok(new { message = "Product Deleted" });
            }
            return BadRequest();

        }

        [HttpGet]
        [Route("AllProduct")]
        public async Task<List<Products>> getForAdmin()
        {
            var products = await _Product.getProductsForAdmin();
            //var timerManager = new TimerManager(() =>_hub.Clients.All.SendAsync("transferAdmindata",_Product.GetProductsForAdmin()));
            return products;
        }

        [HttpGet]
        [Route("SignalR")]
        public IActionResult checkingSignalR()
        {
            var timerManager = new TimerManager(() => _hub.Clients.All.SendAsync("transferAdmindata", _Product.checkingForSignal()));
            return Ok(new { message = "request" });
        }


        public static List<ChartModel> getData()
        {
            var r = new Random();
            return new List<ChartModel>()
                {
                   new ChartModel { Data = new List<int> { r.Next(1, 40) }, Label = "Data1" },
                   new ChartModel { Data = new List<int> { r.Next(1, 40) }, Label = "Data2" },
                   new ChartModel { Data = new List<int> { r.Next(1, 40) }, Label = "Data3" },
                   new ChartModel { Data = new List<int> { r.Next(1, 40) }, Label = "Data4" }
                };
        }



    }
}