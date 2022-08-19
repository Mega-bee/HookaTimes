using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

    public partial class Profile_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }

        public string Gender { get; set; }
        public int? GenderId { get; set; }
        public DateTime? BirthDate { get; set; }


        public string AboutMe { get; set; }
        public string MaritalStatus { get; set; }
        public int? MaritalStatusId { get; set; }

        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public string BodyType { get; set; }
        public int? BodyTypeId { get; set; }
        public string Eyes { get; set; }
        public int? EyesId { get; set; }
        public string Hair { get; set; }
        public int? HairId { get; set; }
        //public string Education { get; set; }
        public string Profession { get; set; }
        public string Interests { get; set; }
        public string Hobbies { get; set; }
        public string SocialMediaLink1 { get; set; }
        public string SocialMediaLink2 { get; set; }
        public string SocialMediaLink3 { get; set; }
        public bool? IsAvailable { get; set; }
        public List<BuddyProfileAddressVM> Addresses { get; set; }
        public List<BuddyProfileEducationVM> Education { get; set; }
        public List<BuddyProfileExperienceVM> Experience { get; set; }

    }
    public partial class CompleteProfile_VM
    {
        public IFormFile ImageFile { get; set; }
        public string Image { get; set; }
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

        public List<BuddyProfileEducationPutVM> Education { get; set; }
        public List<BuddyProfileExperiencePutVM> Experience { get; set; }


    }


    public partial class BuddyProfileAddressVM
    {
        public int Id { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string Building { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        [Required]
        public string Title { get; set; }
        public string Appartment { get; set; }
    }


    public partial class BuddyProfileAddressPutVM : BuddyProfileAddressVM
    {
        public bool IsDeleted { get; set; }
    }

    public partial class BuddyProfileEducationVM
    {
        public int Id { get; set; }
        [StringLength(511)]
        public string University { get; set; }
        [StringLength(511)]
        public string Degree { get; set; }
        [StringLength(255)]
        public string StudiedFrom { get; set; }
        [StringLength(255)]
        public string StudiedTo { get; set; }

    }

    public partial class BuddyProfileEducationPutVM : BuddyProfileEducationVM
    {
        public bool IsDeleted { get; set; }

    }




    public partial class BuddyProfileExperienceVM
    {
        public int Id { get; set; }
        public string Place { get; set; }
        [StringLength(255)]
        public string Position { get; set; }
        [StringLength(255)]
        public string WorkedFrom { get; set; }
        [StringLength(255)]
        public string WorkedTo { get; set; }

    }

    public partial class BuddyProfileExperiencePutVM : BuddyProfileExperienceVM
    {
        public bool IsDeleted { get; set; }

    }


    public partial class ConsumeChangePasswordToken_VM
    {
        public string Password { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }
    }




}