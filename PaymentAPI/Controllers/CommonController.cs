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
    public class CommonController : BaseController
    {

       
        public CommonController(ICommonDomain common)
        {
            this._common = common;

        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Dictionary<string,string>> Get(int id)
        {
            var lst = _common.get(id);
            return lst;
        }

        public void RegisterError(ErrorDetails err)
        {
            _common.RegisterError(err);
        }

    }
}