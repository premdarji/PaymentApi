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
        public async Task<ActionResult<IList<ValidationFailure>>> Post(User model)
        { 
            ValidationResult result = validator.Validate(model);

            if (result.IsValid == false)
            {
                return BadRequest(new { result.Errors });
            }


            var status = await _domain.Post(model);

            if (status == true)
            {
                return Ok(new { message = "User Created" });
            }
            return Ok(new { message="Exist"});

        }


        [HttpPut]
        [Route("Update/{id}")]

        public async Task<ActionResult> Put(int id,UserVM model)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            else
            {
                ValidationResult result = vmvalidator.Validate(model);

                if (result.IsValid == false)
                {
                    return BadRequest(new { result.Errors });
                }


                var status = await _domain.Put(id, model);
                if (status == true)
                {
                    return Ok();
                }
                return BadRequest();
            }

        }



        [HttpGet]
        [Route("GetbyId/{id}")]
        public async Task<ActionResult> GetUserByID(int id)
        {
            var user = await _domain.GetbyID(id);
            if (user!=null)
            {
                return Ok(user);
            }
            return BadRequest(new {message= "User dont exist"});
        }


        [HttpDelete]
        [Route("delete/{id}")]
        public async  Task<ActionResult> Delete(int id)
        {
            var status = await _domain.Delete(id);
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
       


    }
}