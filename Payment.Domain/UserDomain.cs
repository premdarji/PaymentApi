using FluentValidation;
using FluentValidation.Results;
using Payment.Core.Filters;
using Payment.Entity;
using Payment.Entity.DbModels;
using Payment.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Payment.Domain
{
    public class UserDomain : IUserDomain
    {
        ApplicationContext _context;
        IEmailSender _emailSender;
        public UserDomain(ApplicationContext Context,IEmailSender emailSender)
        {
            _context = Context;
            _emailSender = emailSender;
        }

        public async Task<bool> ChangePassword(ChangePasswordVM Model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Email.Equals(Model.Email) && m.Password.Equals(Model.CurrentPassword));
      
            if (user != null)
            {
                user.Password = Model.NewPassword;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<User> Check(LoginVM Model)
        {
            string hashpass = Model.Password;
            var temp = await _context.Users.SingleOrDefaultAsync(m => m.IsActivated==true &&  m.Email.Equals(Model.EmailorPhone) || m.Phone.Equals(Model.EmailorPhone));
            if (temp != null)
            {
                var status = temp.Password.Equals(hashpass);
                if (status == true)
                {
                    return temp;
                }
            }
            return null;
        }

        public async Task<bool> Delete(int Id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId.Equals(Id));
            var test = user != null ? _context.Users.Remove(user) : null;
            //_context.Users.Remove(user);
            var status= await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetbyID(int Id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId.Equals(Id));
            return user;
        }

        public async Task<User> PasswordRecovery(string Email)
        {
            dynamic user = await _context.Users.FirstOrDefaultAsync(m => m.Email.Equals(Email));
            return user;
        }

        public async Task<bool> Post(User Model)
        {
            var check = await _context.Users.FirstOrDefaultAsync(m => m.Email == Model.Email);
            if (check == null)
            {
                Model.IsActivated = false;
                _context.Users.Add(Model);
                var status = await _context.SaveChangesAsync();
                if (status > 0)
                {
                    SendActivationEmail(Model);
                    return true;
                }
                return false;
            }
            return false;
            
           
        }

        public async Task<bool> Put(int Id, UserVM Model)
        {
            var check =  _context.Users.Any(m => m.Email == Model.Email);
            if (check)
            {
                var user = _context.Users.FirstOrDefault(m => m.UserId.Equals(Id));
                user.FirstName = Model.FirstName;
                user.LastName = Model.LastName;
                user.Phone = Model.Phone;
                user.Email = Model.Email;
                user.CityId = Model.cityid;
                user.Address = Model.Address;
                var status = await _context.SaveChangesAsync();
                if (status > 0)
                {
                    return true;
                }
                return false;
            }
            return false;


          
        }

        public async Task<bool> ResetPassword(ResetPasswordVM Model)
        {
           
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Email.Equals(Model.Email));
            user.Password = Model.NewPassword;
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;

        }

        public async Task<User> AdminCheck(LoginVM Model)
        {
            string hashpass = Model.Password;
            var temp = await _context.Users.SingleOrDefaultAsync(m => m.Email.Equals(Model.EmailorPhone)  && m.IsAdmin==true);
            if (temp != null)
            {
                var status = temp.Password.Equals(hashpass);
                if (status == true)
                {
                    return temp;
                }

            }

            return null;

        }


        public void SendActivationEmail(User Model)
        {
            var message = new Message(Model.Email, "Activation link", "this email contains activation link, click <a href='http://localhost:4200/Activate/"+Model.UserId+"' target='_blank'>here.</a>");
             _emailSender.SendEmail(message);
        }

        public async Task<bool> ActivateUser(int id)
        {
            var user =await _context.Users.Where(x => x.UserId == id).FirstOrDefaultAsync();
            if (user != null)
            {
                user.IsActivated = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
          
        }
    }

    public interface IUserDomain
    {
        Task<User> GetbyID(int Id);
        Task<List<User>> GetAll();

        Task<bool> Post(User Model);

        Task<bool> Put(int Id, UserVM Model);

        Task<bool> Delete(int Id);

        Task<User> Check(LoginVM Model);

        Task<bool> ChangePassword(ChangePasswordVM Model);

        Task<User> PasswordRecovery(string Email);

        Task<bool> ResetPassword(ResetPasswordVM Model);

        Task<User> AdminCheck(LoginVM Model);

        Task<bool> ActivateUser(int id);

        void SendActivationEmail(User Model);
    }
}
