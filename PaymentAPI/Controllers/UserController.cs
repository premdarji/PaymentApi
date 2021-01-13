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
        public async Task<ActionResult<IList<ValidationFailure>>> post(User model)
        { 
            ValidationResult result = validator.Validate(model);
            //  int id = model.CityId.Value; check error

            // string[] myarray = new string[10]; google

         
            if (result.IsValid  == false)
            {
                return BadRequest(new { result.Errors });
            }

            
            var status = await _domain.post(model);

            if (status == true)
            {
                return Ok(new { message = "User Created" });
            }
            return Ok(new { message="Exist"});

        }


        [HttpPut]
        [Route("Update/{id}")]

        public async Task<ActionResult> put(int id,UserVM model)
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


                var status = await _domain.put(id, model);
                if (status == true)
                {
                    return Ok();
                }
                return BadRequest();
            }

        }



        [HttpGet]
        [Route("GetbyId/{id}")]
        public async Task<ActionResult> getUserByID(int id)
        {
            var user = await _domain.getbyID(id);
            if (user!=null)
            {
                return Ok(user);
            }
            return BadRequest(new {message= "User dont exist"});
           
        }


        [HttpDelete]
        [Route("delete/{id}")]
        public async  Task<ActionResult> delete(int id)
        {
            var status = await _domain.delete(id);
            if (status == true)
            {
                return Ok(new { message = "user deleted" });
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<User>>> getAll()
        {
            var users = await _domain.getAll();
            return users;
        }
       
        [HttpPost]
        [Route("Activate/{id}")]
        public async Task<ActionResult> activateUser(int id)
        {
            var result = await _domain.activateUser(id);
            if (result == true)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("UpdateWallet")]
        public async Task<ActionResult> updateWallet(WalletVM wallet)
        {
            var result = await _domain.updateWallet(wallet);
            return Ok(result);
        }   

    }

}