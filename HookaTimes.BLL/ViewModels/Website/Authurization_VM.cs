using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

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



    public partial class CompleteProfileMVC_VM
    {
        public IFormFile ImageFile { get; set; }
        //public string Image { get; set; }
        public string AboutMe { get; set; }
        public DateTime Birthdate { get; set; }
        public int GenderId { get; set; }
        public int MaritalStatus { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public int BodyType { get; set; }
        public int Eyes { get; set; }
        public int Hair { get; set; }
        //public int Education { get; set; }
        public string Profession { get; set; }
        public string Interests { get; set; }
        public string Hobbies { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SocialMediaLink1 { get; set; }
        public string SocialMediaLink2 { get; set; }
        public string SocialMediaLink3 { get; set; }
        //public List<BuddyProfileAddressPutVM> Addresses { get; set; }
        //public List<BuddyProfileEducationPutVM> Education { get; set; }
        //public List<BuddyProfileExperiencePutVM> Experience { get; set; }


    }



}
