﻿using System.ComponentModel.DataAnnotations;

namespace HookaTimes.BLL.ViewModels.Website
{
    public partial class EmailSignUpMVC_VM
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [StringLength(100)]
        [Display(Name = "Last Name")]
        [Required]

        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        [Required]

        public string PhoneNumber { get; set; }
    }


    public partial class EmailSignInMVC_VM
    {
        [Required]

        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }



    public partial class PasswordMVC_VM
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]

        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]

        public string NewPassword { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
