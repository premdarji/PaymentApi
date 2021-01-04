using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Payment.Core.Filters;
using Payment.Domain;
using Payment.Entity.DbModels;
using Payment.Entity.ViewModels;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [EnableCors("AllowOrigin")]
    public class UserController : BaseController
    {
        //private IUserDomain _domain;
        UserValidator validator = new UserValidator();
        UserVMvalidator vmvalidator = new UserVMvalidator();
        Loginvalidator loginvalidator = new Loginvalidator();

         private readonly ApplicationSettings _setting;
        public UserController(IUserDomain _domain,IOptions<ApplicationSettings> setting)
        {
            this._domain = _domain;
          _setting = setting.Value;
                
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<IList<ValidationFailure>>> Post(User Model)
        { 
            ValidationResult result = validator.Validate(Model);
            //  int id = model.CityId.Value; check error

            // string[] myarray = new string[10]; google

         
            if (result.IsValid  == false)
            {
                return BadRequest(new { result.Errors });
            }

            
            var status = await _domain.Post(Model);

            if (status == true)
            {
                return Ok(new { message = "User Created" });
            }
            return Ok(new { message="Exist"});

        }


        [HttpPut]
        [Route("Update/{id}")]

        public async Task<ActionResult> Put(int Id,UserVM Model)
        {
          
            if (Id == 0)
            {
                return BadRequest();
            }
            else
            {
                ValidationResult result = vmvalidator.Validate(Model);

                if (result.IsValid == false)
                {
                    return BadRequest(new { result.Errors });
                }


                var status = await _domain.Put(Id, Model);
                if (status == true)
                {
                    return Ok();
                }
                return BadRequest();
            }

        }



        [HttpGet]
        [Route("GetbyId/{id}")]
        public async Task<ActionResult> GetUserByID(int Id)
        {
            var user = await _domain.GetbyID(Id);
            if (user!=null)
            {
                return Ok(user);
            }
            return BadRequest(new {message= "User dont exist"});
           
        }


        [HttpDelete]
        [Route("delete/{id}")]
        public async  Task<ActionResult> Delete(int Id)
        {
            var status = await _domain.Delete(Id);
            if (status == true)
            {
                return Ok(new { message = "user deleted" });
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _domain.GetAll();
            return users;
        }
       
        [HttpPost]
        [Route("Activate/{id}")]
        public async Task<ActionResult> ActivateUser(int id)
        {
            var result = await _domain.ActivateUser(id);
            if (result == true)
            {
                return Ok();
            }
            return BadRequest();
        }



       

    }




    public class Apiresponsemodel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public dynamic  ResponseData { get; set; } //check generic return type
    }
}