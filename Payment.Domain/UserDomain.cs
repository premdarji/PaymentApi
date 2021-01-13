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
using System.Security.Cryptography;
using System.IO;

namespace Payment.Domain
{
    public class UserDomain : IUserDomain
    {
        ApplicationContext _context;
        IEmailSender _emailSender;
        public UserDomain(ApplicationContext context,IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<bool> changePassword(ChangePasswordVM model)
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

        public async Task<User> check(LoginVM model)
        {
            string hashpass = model.Password;
            var temp = await _context.Users.SingleOrDefaultAsync(m => m.IsActivated==true &&  m.Email.Equals(model.EmailorPhone) || m.Phone.Equals(model.EmailorPhone));
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

        public async Task<bool> delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            var test = user != null ? _context.Users.Remove(user) : null;
            //_context.Users.Remove(user);
            var status= await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<User>> getAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> getbyID(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }

        public async Task<User> passwordRecovery(string email)
        {
            dynamic user = await _context.Users.FirstOrDefaultAsync(m => m.Email.Equals(email));
            return user;
        }

        public async Task<bool> post(User model)
        {
            var check = await _context.Users.FirstOrDefaultAsync(m => m.Email == model.Email);
            if (check == null)
            {
                model.IsActivated = false;
                model.WalletAmt = 0;
                _context.Users.Add(model);
                var status = await _context.SaveChangesAsync();
                if (status > 0)
                {
                    sendActivationEmail(model);
                    return true;
                }
                return false;
            }
            return false;
            
        }

        public async Task<bool> put(int id, UserVM model)
        {
            var check =  _context.Users.Any(m => m.Email == model.Email);
            if (check)
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

        public async Task<bool> resetPassword(ResetPasswordVM model)
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

        public async Task<User> adminCheck(LoginVM model)
        {
            string hashpass = model.Password;
            var temp = await _context.Users.SingleOrDefaultAsync(m => m.Email.Equals(model.EmailorPhone)  && m.IsAdmin==true);
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


        public void sendActivationEmail(User model)
        {
            string key = "01234567890123456789012345678901";
            var encryptedId = encryptString(key, Convert.ToString(model.UserId));
            var message = new Message(model.Email, "Activation link", "this email contains activation link, click <a href='http://localhost:4200/Activate/"+model.UserId+"' target='_blank'>here.</a>");
             _emailSender.SendEmail(message);
        }

        public async Task<bool> activateUser(int id)
        {
            var user =await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsActivated = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
          
        }

        public async Task<decimal> updateWallet(WalletVM wallet)
        {
            var user = await _context.Users.FindAsync(wallet.Id);
            if (user != null)
            {
                user.WalletAmt += wallet.Amt;
                var status=await _context.SaveChangesAsync();
                if (status > 0)
                {
                    return user.WalletAmt;
                }
                return user.WalletAmt;
            }
            return user.WalletAmt;
        }


        public string encryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }


    }

    public interface IUserDomain
    {
        Task<User> getbyID(int id);
        Task<List<User>> getAll();

        Task<bool> post(User model);

        Task<bool> put(int id, UserVM model);

        Task<bool> delete(int id);

        Task<User> check(LoginVM model);

        Task<bool> changePassword(ChangePasswordVM model);

        Task<User> passwordRecovery(string email);

        Task<bool> resetPassword(ResetPasswordVM model);

        Task<User> adminCheck(LoginVM model);

        Task<bool> activateUser(int id);

        Task<decimal> updateWallet(WalletVM wallet);

        void sendActivationEmail(User model);

        string encryptString(string key, string plainText);
    }
}
