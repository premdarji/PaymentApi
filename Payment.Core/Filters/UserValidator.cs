using FluentValidation;
using Payment.Entity.DbModels;
using Payment.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Core.Filters
{
    public  class UserValidator: AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(m => m.Email).NotEmpty().EmailAddress();
            RuleFor(m => m.Phone).MaximumLength(10).NotEmpty();
            RuleFor(m => m.Password).NotEmpty().MinimumLength(6);
           
        }
    }
    public class UserVMvalidator : AbstractValidator<UserVM>
    {
        public UserVMvalidator()
        {
            RuleFor(m => m.Email).NotEmpty().EmailAddress();
            RuleFor(m => m.Email).NotEmpty().EmailAddress();
            RuleFor(m => m.Phone).MaximumLength(10).NotEmpty();
            RuleFor(m => m.UserID).NotEmpty();
        }
    }

    public class Loginvalidator : AbstractValidator<LoginVM>
    {
        public Loginvalidator()
        {
            RuleFor(m => m.EmailorPhone).NotEmpty();
            RuleFor(m => m.Password).NotEmpty();
        }
    }


    public class ChangePasswordValidator : AbstractValidator<ChangePasswordVM>
    {
        public ChangePasswordValidator()
        {
            RuleFor(m => m.Email).NotEmpty();
            RuleFor(m => m.CurrentPassword).NotEmpty();
            RuleFor(m => m.NewPassword).NotEmpty();
            RuleFor(m => m.ConfirmPassword).NotEmpty().Equal(m => m.NewPassword);
        }
    }

    public class ResetPasswordValidator : AbstractValidator<ResetPasswordVM>
    {
        public ResetPasswordValidator()
        {
            RuleFor(m => m.Email).NotEmpty();
            RuleFor(m => m.NewPassword).NotEmpty();
            RuleFor(m => m.ConfirmPassword).NotEmpty().Equal(m=>m.NewPassword);
        }
    }



    public class ProductValidator : AbstractValidator<Products>
    {
        public ProductValidator()
        {
            //RuleFor(m => m.ImageUrl).NotEmpty();
            RuleFor(m => m.Price).NotEmpty();
           // RuleFor(m => m.CategoryId).NotEmpty();
            RuleFor(m => m.Name).NotEmpty();
        }
    }


    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
        }
    }

}
