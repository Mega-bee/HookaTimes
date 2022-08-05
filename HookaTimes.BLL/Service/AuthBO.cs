using AutoMapper;
using HookaTimes.BLL.Enums;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.DAL;
using HookaTimes.DAL.Data;
using HookaTimes.DAL.HookaTimesModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
//using HookaTimes.DAL.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class AuthBO : BaseBO, IAuthBO
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HookaDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthBO(IUnitOfWork unit, IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, HookaDbContext context, NotificationHelper notificationHelper, RoleManager<IdentityRole> roleManager) : base(unit, mapper, notificationHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
        }

        #region SignIn
        public async Task<ResponseModel> EmailSignIn(EmailSignIn_VM model)
        {
            ResponseModel responseModel = new ResponseModel();
            ApplicationUser res = await _userManager.FindByEmailAsync(model.Email);
            if (res == null)
            {
                responseModel.StatusCode = 400;
                responseModel.ErrorMessage = "User Doesn't Exist";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            var pass = await _userManager.CheckPasswordAsync(res, model.Password);
            if (!pass)
            {
                responseModel.StatusCode = 400;
                responseModel.ErrorMessage = "Username/Password Combination is Not Correct";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            var signIn = await _signInManager.PasswordSignInAsync(res, model.Password, isPersistent: true, lockoutOnFailure: false);
            if (!signIn.Succeeded)
            {
                responseModel.StatusCode = 500;
                responseModel.ErrorMessage = "Sign In Failed";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            BuddyProfile buddy = _context.BuddyProfiles.Where(x => x.UserId == res.Id).FirstOrDefault();
            var roles = await _userManager.GetRolesAsync(res);
            var claims = Tools.GenerateClaims(res, roles, buddy);
            string JwtToken = Tools.GenerateJWT(claims);

            responseModel.StatusCode = 200;
            responseModel.ErrorMessage = "";
            responseModel.Data = new DataModel
            {
                Data = JwtToken,
                Message = ""
            };
            return responseModel;
        }

        #endregion


        #region Refresh FCM
        public async Task<ResponseModel> RefreshFcmToken(string uid, string token)
        {
            ResponseModel responseModel = new ResponseModel();
            ApplicationUser user = await _userManager.FindByIdAsync(uid);
            if (user == null)
            {
                responseModel.ErrorMessage = "User not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            user.FcmToken = token;
            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded)
            {
                responseModel.ErrorMessage = "Failed to refresh fcm token";
                responseModel.StatusCode = 400;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = "", Message = "Fcm token refreshed succesfully" };
            return responseModel;

        }

        #endregion



        #region UpdateProfile
        public async Task<ResponseModel> CompleteProfile(CompleteProfile_VM model, string uid, HttpRequest Request)
        {

            try
            {
                ApplicationUser aspuser = await _userManager.FindByIdAsync(uid);
                //AccProfile user = _context.AccProfiles.Include(x => x.Gender).Include(x => x.Role).Where(x => x.UserId == uid && x.IsDeleted == false).FirstOrDefault();
                ResponseModel responseModel = new ResponseModel();

                if (aspuser == null)
                {
                    responseModel.StatusCode = 404;
                    responseModel.ErrorMessage = "User was not Found";
                    responseModel.Data = new DataModel { Data = "", Message = "" };
                    return responseModel;
                }

                BuddyProfile currProfile = new BuddyProfile()
                {
                    Profession = model.Profession,
                    Weight = model.Weight,
                    MaritalStatus = model.MaritalStatus,
                    Longitude = model.Longitude,
                    About = model.AboutMe,
                    BodyType = model.BodyType,
                    Education = model.Education,
                    CreatedDate = DateTime.UtcNow,
                    DateOfBirth = model.Birthdate != default ? model.Birthdate : model.Birthdate = new DateTime(),
                    Eyes = model.Eyes,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    GenderId = model.GenderId,
                    Hair = model.Hair,
                    Height = model.Height,
                    Hobbies = model.Hobbies,
                    Interests = model.Interests,
                    Latitude = model.Latitude,

                };


                IFormFile file = model.ImageFile;
                if (file != null)
                {
                    string NewFileName = await Helpers.SaveFile("wwwroot/Images/Buddies", file);

                    currProfile.Image = NewFileName;
                }


                await _context.SaveChangesAsync();

                Profile_VM userProfile = new Profile_VM()
                {
                    Name = currProfile.FirstName + " " + currProfile.LastName ?? "",
                    AboutMe = currProfile.About ?? "",
                    Email = aspuser.Email ?? "",
                    BirthDate = (DateTime)currProfile.DateOfBirth != default ? (DateTime)currProfile.DateOfBirth : new DateTime(),
                    GenderId = (int)currProfile.GenderId,
                    Gender = Enum.GetName(typeof(GenderEnum), currProfile.GenderId) ?? "",
                    ImageUrl = $"{Request.Scheme}://{Request.Host}/Buddies/{currProfile.Image}",
                    MaritalStatus = Enum.GetName(typeof(MaritalStatusEnum), currProfile.MaritalStatus) ?? "",
                    Height = (decimal)currProfile.Height,
                    Weight = (decimal)currProfile.Weight,
                    BodyType = Enum.GetName(typeof(BodyTypeEnum), currProfile.BodyType) ?? "",
                    Eyes = Enum.GetName(typeof(EyeEnum), currProfile.Eyes) ?? "",
                    Hair = Enum.GetName(typeof(HairEnum), currProfile.Hair) ?? "",
                    //Education = aspuser.Education ?? "",
                    //Profession = aspuser.Profession ?? "",
                    //Interests = aspuser.Interests ?? "",
                    //Hobbies = aspuser.Hobbies ?? "",


                };
                responseModel.StatusCode = 200;
                responseModel.ErrorMessage = "";
                responseModel.Data = new DataModel
                {
                    Data = userProfile,
                    Message = ""
                };
                return responseModel;

            }
            catch (Exception ex)
            {
                ResponseModel responseModel = new ResponseModel();
                responseModel.StatusCode = 500;
                responseModel.ErrorMessage = ex.ToString();
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
                throw;
            }

        }

        #endregion


        #region SignUp
        public async Task<ResponseModel> SignUpWithEmail(EmailSignUp_VM model, HttpRequest Request)
        {
            await CheckRoles();

            ResponseModel responseModel = new ResponseModel();
            ApplicationUser oldUser = await _userManager.FindByEmailAsync(model.Email);
            if (oldUser != null)
            {
                responseModel.StatusCode = 400;
                responseModel.ErrorMessage = "User already exists";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            IdentityResult res = await CreateUser(model);

            if (!res.Succeeded)
            {
                responseModel.StatusCode = 400;
                responseModel.ErrorMessage = res.Errors.FirstOrDefault().Description;
                responseModel.Data = new DataModel
                {
                    Data = "",
                    Message = ""
                };
            }

            //Create buddy Profile
            BuddyProfile buddy = await CreateBuddyProfile(model);

            //Get the Identity User Profile so it can get its claims and roles
            ApplicationUser newUser = await _userManager.FindByEmailAsync(model.Email);

            var roles = await _userManager.GetRolesAsync(newUser);
            var claims = Tools.GenerateClaims(newUser, roles, buddy);
            string JwtToken = Tools.GenerateJWT(claims);

            responseModel.StatusCode = 200;
            responseModel.ErrorMessage = "";
            responseModel.Data = new DataModel
            {
                Data = JwtToken,
                Message = ""
            };
            return responseModel;
        }


        public async Task<IdentityResult> CreateUser(EmailSignUp_VM model)
        {
            ApplicationUser user = new ApplicationUser();

            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.CreatedDate = DateTime.UtcNow;
            user.IsDeleted = false;
            user.PhoneNumberConfirmed = true;
            user.EmailConfirmed = true;
            IdentityResult res = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, AppSetting.UserRole);
            // check if user creation succeeded
            return res;
        }

        private async Task CheckRoles()
        {
            if (!await _roleManager.RoleExistsAsync(AppSetting.UserRole))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = AppSetting.UserRole, NormalizedName = AppSetting.UserRoleNormalized });
            }
            if (!await _roleManager.RoleExistsAsync(AppSetting.AdminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = AppSetting.AdminRole, NormalizedName = AppSetting.AdminRoleNormalized });
            }
        }

        public async Task<BuddyProfile> CreateBuddyProfile(EmailSignUp_VM model)
        {
            // create user profile and add role based on roleId
            BuddyProfile newProfile = new BuddyProfile();
            newProfile.UserId = _context.AspNetUsers.Where(x => x.Email == model.Email).FirstOrDefault().Id;
            newProfile.IsDeleted = false;
            newProfile.CreatedDate = DateTime.UtcNow;
            newProfile.FirstName = model.FirstName;
            newProfile.LastName = model.LastName;
            newProfile.IsAvailable = true;
            newProfile.Image = "use-placeholder.png";
            //newProfile.GenderId = 1;
            // add the characteristics to the BuddyProfiles
            await _context.BuddyProfiles.AddAsync(newProfile);
            await _context.SaveChangesAsync();
            return newProfile;
        }

        public string GenerateJWT(List<Claim> claims)
        {
            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fdjfhjehfjhfuehfbhvdbvjjoq8327483rgh"));
            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "https://localhost:44310",
               audience: "https://localhost:44310",
               claims: claims,
               notBefore: DateTime.Now,
               expires: DateTime.Now.AddYears(1),
               signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
               );
            string JwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return JwtToken;
        }

        #endregion


        #region Forget Pass
        ///// Reset pass with sms


        public async Task<ResponseModel> SendChangePasswordToken(string identifier)
        {

            ResponseModel responseModel = new ResponseModel();
            var UID = _context.AspNetUsers.Where(x => x.Email == identifier).FirstOrDefault().Id;
            //string UID = User.Claims.Where(x => x.Type == "UID").FirstOrDefault().Value;
            var user = await _userManager.FindByIdAsync(UID);

            if (user is null)
            {
                responseModel.StatusCode = 404;
                responseModel.ErrorMessage = "User was not Found";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            var token = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose");

            string content = $"Dear Hooka Buddy \n For Security reasons,User reset code in your app to reser password \n Your Reset Password Code is : {token} \n Thank you";

            bool isEmailSent = await Tools.SendEmailAsync(identifier, "Reset Password", content);
            if (isEmailSent)
            {
                responseModel.StatusCode = 200;
                responseModel.ErrorMessage = "";
                responseModel.Data = new DataModel
                {
                    Data = "",
                    Message = "Details to reset password have been sent to email"
                };
                return responseModel;
            }
            responseModel.StatusCode = 500;
            responseModel.ErrorMessage = "Failed To Send Email";
            responseModel.Data = new DataModel { Data = "", Message = "" };
            return responseModel;
        }




        public async Task<ResponseModel> ConsumeChangePasswordToken(ConsumeChangePasswordToken_VM model)
        {
            ResponseModel responseModel = new ResponseModel();

            var UID = _context.AspNetUsers.Where(x => x.Email == model.Email).FirstOrDefault().Id;
            //string UID = User.Claims.Where(x => x.Type == "UID").FirstOrDefault().Value;
            var user = await _userManager.FindByIdAsync(UID);

            if (user is null)
            {
                responseModel.StatusCode = 404;
                responseModel.ErrorMessage = "User was not Found";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }




            var tokenVerified = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose", model.Token);

            if (!tokenVerified)
            {
                responseModel.StatusCode = 404;
                responseModel.ErrorMessage = "Bad Token";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);//new token for reseting password
            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
            if (!result.Succeeded)
            {
                responseModel.StatusCode = 400;
                responseModel.ErrorMessage = "Rest Password Faild";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            responseModel.StatusCode = 200;
            responseModel.ErrorMessage = "";
            responseModel.Data = new DataModel { Data = "", Message = "Password has been changed" };
            return responseModel;


        }


        #endregion


        #region Reset Pass
        public async Task<ResponseModel> ResetPassword(ResetPassword_VM model, string uid)
        {
            var user = await _userManager.FindByIdAsync(uid);
            ResponseModel responseModel = new ResponseModel();
            if (user == null)
            {



                responseModel.StatusCode = 400;
                responseModel.ErrorMessage = "User Doesn't Exist";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }


            if (!await _userManager.CheckPasswordAsync(user, model.OldPassword))
            {

                responseModel.StatusCode = 400;
                responseModel.ErrorMessage = "Old Password Is Invalid";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            if (model.NewPassword == model.OldPassword)
            {

                responseModel.StatusCode = 400;
                responseModel.ErrorMessage = "New password can't be the same as old password";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                responseModel.StatusCode = 400;
                responseModel.ErrorMessage = "Passwords don't match";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);
            var decodedToken = WebEncoders.Base64UrlDecode(validToken);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);
            if (result.Succeeded)
            {
                responseModel.StatusCode = 200;
                responseModel.ErrorMessage = "";
                responseModel.Data = new DataModel
                {
                    Data = "",
                    Message = "Password has been reset succesfully"
                };
                return responseModel;
            }

            responseModel.StatusCode = 400;
            responseModel.ErrorMessage = result.Errors.Select(e => e.Description).FirstOrDefault();
            responseModel.Data = new DataModel { Data = "", Message = "" };
            return responseModel;

        }

        #endregion



        #region OTP
        public async Task<ResponseModel> GenerateOtp(string Email)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                //phone = Helpers.RemoveCountryCode(phone);

                var currphoneOtp = await _context.EmailOtps.Where(x => x.Email == Email).FirstOrDefaultAsync();
                if (currphoneOtp != null)
                {
                    var resend = await ResendOtp(Email);

                    responseModel.StatusCode = 200;
                    responseModel.ErrorMessage = "";
                    responseModel.Data = new DataModel
                    {
                        Data = "",
                        Message = "Otp has been Sent again to Email"
                    };
                    return responseModel;
                }
                var otp = Helpers.Generate_otp();

                EmailOtp emailOtp = new EmailOtp()
                {
                    Email = Email,
                    Otp = otp,
                };

                await _context.EmailOtps.AddAsync(emailOtp);
                await _context.SaveChangesAsync();

                string content = $"Deare Hooka Buddy \n Your Verification Pin is : {otp} \n Thank you for choosing Hooka Times";

                //Helpers.SendSMS(phone, content);
                await Tools.SendEmailAsync(Email, "Hooka OTP", content);

                responseModel.StatusCode = 200;
                responseModel.ErrorMessage = "";
                responseModel.Data = new DataModel
                {
                    Data = "",
                    Message = "Otp has been Sent to you email"
                };
                return responseModel;
            }
            catch (Exception ex)
            {

                responseModel.StatusCode = 500;
                responseModel.ErrorMessage = ex.ToString();
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
                throw;

            }


        }


        public async Task<ResponseModel> ConfirmOtp(string otp, string Email)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                //phone = Helpers.RemoveCountryCode(phone);

                var EmailOtp = await _context.EmailOtps.Where(x => x.Email == Email).FirstOrDefaultAsync();

                int dbotp = Convert.ToInt32(EmailOtp.Otp);
                int useotp = Convert.ToInt32(otp);

                if (dbotp == useotp)
                {
                    responseModel.StatusCode = 200;
                    responseModel.ErrorMessage = "";
                    responseModel.Data = new DataModel
                    {
                        Data = true,
                        Message = "Otp ConfirmOtp"
                    };
                    return responseModel;
                }

                else
                {
                    // generate otp again
                    var newotp = Helpers.Generate_otp();
                    EmailOtp.Otp = newotp;
                    responseModel.StatusCode = 401;
                    responseModel.ErrorMessage = "";
                    responseModel.Data = new DataModel
                    {
                        Data = "",
                        Message = "Otp is Incorrect"
                    };
                    return responseModel;
                }
            }
            catch (Exception ex)
            {
                responseModel.StatusCode = 500;
                responseModel.ErrorMessage = ex.ToString();
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
                throw;

            }


        }


        public async Task<ResponseModel> ResendOtp(string Email)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {

                //phone = Helpers.RemoveCountryCode(phone);


                var EmailOtp = await _context.EmailOtps.Where(x => x.Email == Email).FirstOrDefaultAsync();


                var otp = Helpers.Generate_otp();

                EmailOtp.Otp = otp;

                await _context.SaveChangesAsync();
                string content = $"Deare Hooka Buddy \n Your Verification Pin is : {otp} \n Thank you for choosing Hooka Times";

                //Helpers.SendSMS(phone, content);
                await Tools.SendEmailAsync(Email, "Hooka OTP", content);

                responseModel.StatusCode = 200;
                responseModel.ErrorMessage = "";
                responseModel.Data = new DataModel
                {
                    Data = "",
                    Message = "Otp has been Resent to your Email"
                };
                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.StatusCode = 500;
                responseModel.ErrorMessage = ex.ToString();
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
                throw;

            }

        }
        #endregion






    }
}
