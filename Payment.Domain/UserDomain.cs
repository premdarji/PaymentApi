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
        public UserDomain(ApplicationContext context)
        {
            _context = context;                
        }

        public async Task<bool> ChangePassword(ChangePasswordVM model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Email.Equals(model.Email) && m.Password.Equals(model.CurrentPassword));
      
            if (user != null)
            {
                user.Password = model.NewPassword;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<User> Check(LoginVM model)
        {
            string hashpass = model.Password;
            var temp = await _context.Users.SingleOrDefaultAsync(m => m.Email.Equals(model.EmailorPhone) || m.Phone.Equals(model.EmailorPhone));
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

        public async Task<bool> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId.Equals(id));
            _context.Users.Remove(user);
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

        public async Task<User> GetbyID(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId.Equals(id));
            return user;
        }

        public async Task<User> PasswordRecovery(string Email)
        {
            dynamic user = await _context.Users.FirstOrDefaultAsync(m => m.Email.Equals(Email));
            return user;
        }

        public async Task<bool> Post(User model)
        {
            var check = await _context.Users.FirstOrDefaultAsync(m => m.Email == model.Email);
            if (check == null)
            {
                _context.Users.Add(model);
                var status = await _context.SaveChangesAsync();
                if (status > 0)
                {
                    return true;
                }
                return false;

            }
            else
            {
                return false;
            }
           
        }

        public async Task<bool> Put(int id, UserVM model)
        {
            var check =  _context.Users.FirstOrDefault(m => m.Email == model.Email);
            if (check == null)
            {
                var user = _context.Users.FirstOrDefault(m => m.UserId.Equals(id));
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Phone = model.Phone;
                user.Email = model.Email;
                user.CityId = model.cityid;
                user.Address = model.Address;
                var status = await _context.SaveChangesAsync();
                if (status > 0)
                {
                    return true;
                }
                return false;
            }
            return false;


          
        }

        public async Task<bool> ResetPassword(ResetPasswordVM model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Email.Equals(model.Email));
            user.Password = model.NewPassword;
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;

        }
    }

    public interface IUserDomain
    {
        Task<User> GetbyID(int id);
        Task<List<User>> GetAll();

        Task<bool> Post(User model);

        Task<bool> Put(int id, UserVM model);

        Task<bool> Delete(int id);

        Task<User> Check(LoginVM model);

        Task<bool> ChangePassword(ChangePasswordVM model);

        Task<User> PasswordRecovery(string Email);

        Task<bool> ResetPassword(ResetPasswordVM model);
    }
}
