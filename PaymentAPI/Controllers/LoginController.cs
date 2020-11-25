using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Payment.Core.Filters;
using Payment.Domain;
using Payment.Entity.ViewModels;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        //UserValidator validator = new UserValidator();
        //UserVMvalidator vmvalidator = new UserVMvalidator();
        Loginvalidator loginvalidator = new Loginvalidator();
        ChangePasswordValidator passwordvalidator = new ChangePasswordValidator();
        ResetPasswordValidator resetvalidator = new ResetPasswordValidator();
        public LoginController(IUserDomain domain)
        {
            this._domain = domain;
        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login(LoginVM model)
        {
            ValidationResult result = loginvalidator.Validate(model);

            if (result.IsValid == false)
            {
                return BadRequest(new { result.Errors });
            }

            var user = await _domain.Check(model);

            if (user != null)
            {
                var token = CreateToken(user.UserId, user.Email);
                return Ok(token);
            }
            else
                return Ok(new { message="invalid credentials"});
        }


        [Authorize]
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordVM model)
        {
            ValidationResult result = passwordvalidator.Validate(model);
            if (result.IsValid == false)
            {
                return BadRequest(new { result.Errors });
            }

            var status = await _domain.ChangePassword(model);
            if (status == true)
            {

                return Ok(new { message = "Password changed" });
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<ActionResult<string>> ForgotPassword([FromQuery]string Email)
        {
            var user = await _domain.PasswordRecovery(Email);
            if (user != null)
            {
                var token =  CreateToken(user.UserId, user.Email);
                return Ok(token);
            }
            return Ok(new { message="not varified"});
        }


        [Authorize]
        [HttpPost]
        [Route("Reset")]
        public async Task<ActionResult> ResetPassword(ResetPasswordVM model)
        {
            var status = await _domain.ResetPassword(model);
            if (status == true)
            {
                return Ok(new {message= "Password Reset" });
            }
            return BadRequest();
        }


        public ActionResult<string> CreateToken(int id,string email)
        {
            //var id = user.UserId.ToString();
            var Id = id.ToString();
            var Email = email.ToString();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim("UserID",Id),
                        new Claim("Email",Email)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456")), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return  token ;
        }

    }
}