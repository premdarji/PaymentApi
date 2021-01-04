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

        public ProductController(IProductDomain Product, IHubContext<DataHub> Hub)
        {
            this._Product = Product;
            _hub = Hub;
        }

        [HttpGet]
        [Route("GetAll/{id}/{pagenumber}/{pagesize}")]
        public async Task<ActionResult> GetAll(int Id,int Pagenumber,int Pagesize)
        {
            Pagesize=Pagenumber==5?Pagesize=20:Pagesize=6;
            var products =await _Product.GetAll(Id,Pagenumber,Pagesize);
            if (products.Count> 0)
            {
                return Ok(products);
            }
            return Ok(new { message = "nodata" });
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult> GetById(int Id)
        {
            var product =await _Product.GetById(Id);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();

        }

        [HttpGet]
        [Route("GetByCategory/{id}")]
        public async Task<ActionResult> GetByCategory(int Id)
        {
            var product = await _Product.GetByCategory(Id);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();

        }


        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> Post(Products Model)
        {

            ValidationResult result = validator.Validate(Model);

            if (result.IsValid == false)
            {
                return BadRequest(new { result.Errors });
            }

            var status = await _Product.Post(Model);
            if (status==true)
            {
                return Ok(new { message = "Product Added" });
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult> Put(Products Model)
        {

            ValidationResult result = validator.Validate(Model);

            if (result.IsValid == false)
            {
                return BadRequest(new { result.Errors });
            }

            var status = await _Product.Put(Model);
            if (status == true)
            {
                return Ok(new { message = "Product Updated" });
            }
            return BadRequest();

        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var status = await _Product.Delete(Id);
            if (status == true)
            {
                return Ok(new { message = "Product Deleted" });
            }
            return BadRequest();

        }

        [HttpGet]
        [Route("AllProduct")]
        public async Task<List<Products>> GetForAdmin()
        {
            var products = await _Product.GetProductsForAdmin();
            //var timerManager = new TimerManager(() =>_hub.Clients.All.SendAsync("transferAdmindata",_Product.GetProductsForAdmin()));
            return products;
        }

        [HttpGet]
        [Route("SignalR")]
        public IActionResult CheckingSignalR()
        {
            var timerManager = new TimerManager(() => _hub.Clients.All.SendAsync("transferAdmindata", _Product.CheckingForSignal()));
            return Ok(new { message = "request" });
        }


        public static List<ChartModel> GetData()
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