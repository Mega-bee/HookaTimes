﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
    //public partial class EmailSignIn_VM
    //{
    //    [Required]
    //    [MinLength(10)]
    //    public string Email { get; set; }
    //    [MinLength(6)]
    //    public string Password { get; set; }
    //    public string DeviceToken { get; set; }
    //}

    public partial class User_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
    }

    public partial class EmailSignUp_VM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "Gender is missing")]
        //public int GenderId { get; set; }

        //[Required(ErrorMessage = "Birthdate is missing")]
        //public DateTime Birthdate { get; set; }

        //public IFormFile ImageFile { get; set; }
        public string DeviceToken { get; set; }

    }

    public partial class Profile_VM
    {
        public int Id { get; set; }
        // public string Uid { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        // public int OrderListId { get; set; }
        // public bool IsActive { get; set; }
        public string Gender { get; set; }
        public int GenderId { get; set; }
        public DateTime BirthDate { get; set; }
        public string Token { get; set; }
        public bool IsProfileComplete { get; set; }

    }

    public partial class EmailSignIn_VM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string DeviceToken { get; set; }


    }

    public partial class ResetPassword_VM
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public partial class ResetPasswordFromEmail_VM
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]

        [StringLength(50, MinimumLength = 5)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        [Compare("NewPassword", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }
    }

    public partial class UpdateProfile_VM
    {
        public string Name { get; set; }
        //public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public partial class CheckifExist_VM
    {
        public bool Phone { get; set; }
        public bool Email { get; set; }
    }

    public partial class CompleteProfile_VM
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public int GenderId { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthdate { get; set; }
        public IFormFile ImageFile { get; set; }
    }

}
