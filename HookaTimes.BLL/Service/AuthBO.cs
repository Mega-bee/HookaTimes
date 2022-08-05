using AutoMapper;
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

        public async Task<ResponseModel> ForgetPassword(string identifier, HttpRequest Request)
        {

            ResponseModel responseModel = new ResponseModel();
            var emailUser = await _userManager.FindByEmailAsync(identifier);
            if (emailUser != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(emailUser);
                var encodedToken = Encoding.UTF8.GetBytes(token);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);

                string url = $"{Request.Scheme}://{Request.Host}/ResetPassword?email={identifier}&token={validToken}";

                bool isEmailSent = await Tools.SendEmailAsync(identifier, "Reset Password", "<h1>Follow the instructions to reset your password</h1>" +
                     $"<p>To reset your password <a href='{url}'>Click here</a></p>");
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

            responseModel.StatusCode = 404;
            responseModel.ErrorMessage = "You will receive an email if your user is registered with Sentinel";
            responseModel.Data = new DataModel { Data = "", Message = "" };
            return responseModel;
        }

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
                    //GenderId = (int)currProfile.Gender,
                    //Gender = currProfile.Gender ?? "",
                    //ImageUrl = $"{Request.Scheme}://{Request.Host}/Uploads/{aspuser.Image}",
                    //PhoneNumber = aspuser.PhoneNumber ?? "",
                    //MaritalStatus = aspuser.MartialStatus ?? "",
                    //Height = (decimal)aspuser.Height,
                    //Weight = (decimal)aspuser.Weight,
                    //BodyType = aspuser.BodyType ?? "",
                    //Eyes = aspuser.Eyes ?? "",
                    //Hair = aspuser.Hair ?? "",
                    //Education = aspuser.Education ?? "",
                    //Profession = aspuser.Profession ?? "",
                    //Interests = aspuser.Interests ?? "",
                    //Hobbies = aspuser.Hobbies ?? "",

                    //Role = user.Role.RoleName ?? "",
                    //Token = "",

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

        #region Reset Pass
        ///// Reset pass with sms


        //public async Task<ResponseModel> SendChangePasswordToken(string Email)
        //{

        //    ResponseModel responseModel = new ResponseModel();
        //    var UID = _context.AspNetUsers.Where(x => x.Email == Email).FirstOrDefault().Id;
        //    //string UID = User.Claims.Where(x => x.Type == "UID").FirstOrDefault().Value;
        //    var user = await _userManager.FindByIdAsync(UID);

        //    if (user is null)
        //    {
        //        responseModel.StatusCode = 404;
        //        responseModel.ErrorMessage = "User was not Found";
        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        return responseModel;
        //    }

        //    var token = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose");

        //    string content = $"Please Enter This Pin To Reset your Password :\n {token}";

        //    _sms.SendSMS(user.PhoneNumber, content);

        //    // _SMSManager.SendResetPasswordToken(token, user.PhoneNumber);
        //    return NoContent();
        //}




        //[HttpPost("resetPassword")]
        ////[ServiceFilter(typeof(ModelValidationFilter))]
        //public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordDto_VM dto)
        //{
        //    var UID = _context.AspNetUsers.Where(x => x.PhoneNumber == dto.PhoneNumber).FirstOrDefault().Id;
        //    //string UID = User.Claims.Where(x => x.Type == "UID").FirstOrDefault().Value;
        //    var user = await _userManager.FindByIdAsync(UID);

        //    if (user is null)
        //        return UnprocessableEntity("invalid user token");//actually user not found

        //    if (dto.NewPassword != dto.ConfirmPassword)
        //        return BadRequest(new { message = "Passwords don't match" });

        //    var tokenVerified = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPasswordPurpose", dto.Token);

        //    if (!tokenVerified)
        //        return UnprocessableEntity("invalid user token");

        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);//new token for reseting password
        //    var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
        //    if (!result.Succeeded)
        //        return UnprocessableEntity("weak password");

        //    return NoContent();
        //}
        #endregion

        //public async Task<ResponseModel> EmailSignIn(EmailSignIn_VM model, HttpRequest Request)
        //{
        //    ResponseModel responseModel = new ResponseModel();


        //    AspNetUser aspres = null;
        //    ApplicationUser res = null;
        //    if (!model.Email.Contains('@'))
        //    {
        //        model.Email = Helpers.RemoveCountryCode(model.Email);
        //    }


        //    if (!string.IsNullOrEmpty(model.Email))
        //    {
        //        aspres = await _context.AspNetUsers.Where(x => x.Email == model.Email || x.PhoneNumber == model.Email).FirstOrDefaultAsync();
        //    }

        //    if (aspres == null)
        //    {



        //        responseModel.StatusCode = 400;
        //        responseModel.ErrorMessage = "User Doesn't Exist";
        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        return responseModel;
        //    }

        //    res = await _userManager.FindByIdAsync(aspres.Id);

        //    //ApplicationUser res = await _userManager.FindByEmailAsync(model.identifier);
        //    bool isVerified = false;
        //    if (res == null)
        //    {



        //        responseModel.StatusCode = 400;
        //        responseModel.ErrorMessage = "User Doesn't Exist";
        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        return responseModel;
        //    }
        //    var pass = await _userManager.CheckPasswordAsync(res, model.Password);
        //    if (!pass)
        //    {



        //        responseModel.StatusCode = 400;
        //        responseModel.ErrorMessage = "Password Is Incorrect";
        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        return responseModel;
        //    }
        //    var signIn = await _signInManager.PasswordSignInAsync(res, model.Password, isPersistent: true, lockoutOnFailure: false);
        //    if (!signIn.Succeeded)
        //    {
        //        if (signIn.IsNotAllowed)
        //        {



        //            responseModel.StatusCode = 500;
        //            responseModel.ErrorMessage = "Sign In Failed, Email Is Not Verified";
        //            responseModel.Data = new DataModel { Data = "", Message = "" };
        //            return responseModel;
        //        }



        //        responseModel.StatusCode = 500;
        //        responseModel.ErrorMessage = "Sign In Failed";
        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        return responseModel;
        //    }
        //    isVerified = await _userManager.IsEmailConfirmedAsync(res);

        //    AccProfile profile = await _context.AccProfiles.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == res.Email && x.IsDeleted == false);
        //    if (profile == null)
        //    {



        //        responseModel.StatusCode = 500;
        //        responseModel.ErrorMessage = "Failed To Fetch Profile";
        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        return responseModel;
        //    }
        //    if (!string.IsNullOrEmpty(model.DeviceToken))
        //    {
        //        res.DeviceToken = model.DeviceToken;
        //        IdentityResult result = await _userManager.UpdateAsync(res);
        //        if (!result.Succeeded)
        //        {


        //            responseModel.StatusCode = 500;
        //            responseModel.ErrorMessage = "Failed to Update Device Token";
        //            responseModel.Data = new DataModel { Data = "", Message = "" };
        //            return responseModel;
        //        }
        //    }

        //    //if (!string.IsNullOrEmpty(model.DeviceToken))
        //    //{
        //    //    bool deviceTokenExists = await _context.UserDeviceTokens.Where(x => x.UserId == res.Id && x.Token == model.DeviceToken).FirstOrDefaultAsync() != null;
        //    //    if (!deviceTokenExists)
        //    //    {
        //    //        var newDeviceToken = new UserDeviceToken()
        //    //        {
        //    //            Token = model.DeviceToken,
        //    //            UserId = res.Id
        //    //        };
        //    //        await _context.UserDeviceTokens.AddAsync(newDeviceToken);
        //    //        await _context.SaveChangesAsync();
        //    //    }
        //    //}

        //    var claims = GenerateClaims(res, profile.Role);
        //    string JwtToken = GenerateJWT(claims);
        //    Profile_VM userProfile = await _context.AccProfiles.Where(x => x.Id == profile.Id).Select(x => new Profile_VM
        //    {
        //        Email = x.Email,
        //        ImageUrl = x.ImageUrl != null ? $"{Request.Scheme}://{Request.Host}/Uploads/{x.ImageUrl}" : $"{Request.Scheme}://{Request.Host}/Uploads/user-placeholder.png",
        //        Role = x.Role.RoleName,
        //        Token = JwtToken,
        //        Id = x.Id,
        //        PhoneNumber = x.PhoneNumber,
        //        //OrderListId= x.OrderLists.FirstOrDefault().Id,
        //        Name = x.Name,
        //        BirthDate = x.BirthDate.HasValue != false ? x.BirthDate.Value : default,
        //        Gender = x.Gender.Name ?? "Not specified",
        //        GenderId = x.GenderId.HasValue ? (int)x.GenderId : 0,
        //        //IsProfileComplete= (bool)x.IsProfileComplete,
        //    }).FirstOrDefaultAsync();

        //    responseModel.StatusCode = 200;
        //    responseModel.ErrorMessage = "";
        //    responseModel.Data = new DataModel
        //    {
        //        Data = userProfile,
        //        Message = ""
        //    };
        //    return responseModel;
        //}

        //public List<Claim> GenerateClaims(ApplicationUser res, AspNetRole role)
        //{
        //    var claims = new List<Claim>()
        //        {
        //        new Claim(JwtRegisteredClaimNames.Email , res.Email ),
        //        new Claim(ClaimTypes.Name , res.UserName),
        //        new Claim("UID",res.Id),
        //        new Claim(ClaimTypes.Role , role.RoleName),
        //        new Claim(ClaimTypes.NameIdentifier, res.Id),
        //        };
        //    return claims;
        //}




        //public async Task<ResponseModel> GetUserProfile(string uid, HttpRequest Request)
        //{
        //    try
        //    {
        //        ResponseModel responseModel = new ResponseModel();
        //        bool isConfirmed = _context.AspNetUsers.Where(x => x.Id == uid).FirstOrDefault().EmailConfirmed;

        //        Profile_VM user = await _context.AccProfiles.Where(x => x.UserId == uid && x.IsDeleted == false)
        //                   .Select(x => new Profile_VM
        //                   {
        //                       Id = x.Id,
        //                       Name = x.Name ?? "",
        //                       Email = x.Email ?? "",
        //                       Gender = x.Gender.Name ?? "",
        //                       GenderId = (int)x.GenderId.Value,
        //                       ImageUrl = x.ImageUrl != null ? $"{Request.Scheme}://{Request.Host}/Uploads/{x.ImageUrl}" : $"{Request.Scheme}://{Request.Host}/Uploads/user-placeholder.png",
        //                       BirthDate = x.BirthDate != null ? x.BirthDate.Value : DateTime.Now,
        //                       PhoneNumber = x.PhoneNumber ?? "",
        //                       Role = x.Role.RoleName,
        //                       Token = "",
        //                   })
        //                   .FirstOrDefaultAsync();

        //        responseModel.StatusCode = 200;
        //        responseModel.ErrorMessage = "";
        //        responseModel.Data = new DataModel
        //        {
        //            Data = user,
        //            Message = ""
        //        };
        //        return responseModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        ResponseModel responseModel = new ResponseModel();



        //        responseModel.StatusCode = 500;
        //        responseModel.ErrorMessage = "Something Went Wrong";
        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        return responseModel;
        //        throw;
        //    }

        //}




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

        //public async Task<ResponseModel> ForgetPassword(string identifier, HttpRequest Request)
        //{

        //    ResponseModel responseModel = new ResponseModel();
        //    var emailUser = await _userManager.FindByEmailAsync(identifier);
        //    if (emailUser != null)
        //    {
        //        var token = await _userManager.GeneratePasswordResetTokenAsync(emailUser);
        //        var encodedToken = Encoding.UTF8.GetBytes(token);
        //        var validToken = WebEncoders.Base64UrlEncode(encodedToken);

        //        string url = $"{Request.Scheme}://{Request.Host}/ResetPassword?email={identifier}&token={validToken}";

        //        bool isEmailSent = await Helpers.SendEmailAsync(identifier, "Reset Password", "<h1>Follow the instructions to reset your password</h1>" +
        //             $"<p>To reset your password <a href='{url}'>Click here</a></p>");
        //        if (isEmailSent)
        //        {
        //            responseModel.StatusCode = 200;
        //            responseModel.ErrorMessage = "";
        //            responseModel.Data = new DataModel
        //            {
        //                Data = "",
        //                Message = "Details to reset password have been sent to email"
        //            };
        //            return responseModel;
        //        }

        //        responseModel.StatusCode = 500;
        //        responseModel.ErrorMessage = "Failed To Send Email";
        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        return responseModel;
        //    }

        //    responseModel.StatusCode = 404;
        //    responseModel.ErrorMessage = "User Not Found";
        //    responseModel.Data = new DataModel { Data = "", Message = "" };
        //    return responseModel;
        //}

        public async Task<ResponseModel> ResetPasswordFromEmail(ResetPasswordFromEmail_VM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            ResponseModel responseModel = new ResponseModel();
            if (user == null)
            {

                responseModel.StatusCode = 404;
                responseModel.ErrorMessage = "User Not Found";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }


            if (model.NewPassword != model.ConfirmPassword)
            {

                responseModel.StatusCode = 500;
                responseModel.ErrorMessage = "Passwords Don't Match";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }


            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
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

        //public async Task<ResponseModel> UpdateProfile(UpdateProfile_VM updatedProfile, string uid, HttpRequest Request)
        //{
        //    ResponseModel responseModel = new ResponseModel();
        //    AccProfile user = await _context.AccProfiles.Where(x => x.UserId == uid && x.IsDeleted == false).FirstOrDefaultAsync();

        //    if (user != null)
        //    {
        //        ApplicationUser res = await _userManager.FindByIdAsync(user.UserId);
        //        if (!string.IsNullOrEmpty(updatedProfile.PhoneNumber))
        //        {
        //            res.PhoneNumber = updatedProfile.PhoneNumber;
        //            user.PhoneNumber = updatedProfile.PhoneNumber;
        //        }
        //        if (!string.IsNullOrEmpty(updatedProfile.Name))
        //        {
        //            user.Name = updatedProfile.Name;
        //        }

        //        // res.GenderId = updatedProfile.GenderId != 0 ? updatedProfile.GenderId : 1;
        //        // user.GenderId = updatedProfile.GenderId != 0 ? updatedProfile.GenderId : 1;
        //        //if (updatedProfile.Email != null)
        //        //{
        //        //    user.Email = updatedProfile.Email;
        //        //    res.UserName = updatedProfile.Email;
        //        //    res.Email = updatedProfile.Email;
        //        //}


        //        if (updatedProfile.BirthDate != new DateTime())
        //        {
        //            user.BirthDate = updatedProfile.BirthDate;
        //            res.BirthDate = updatedProfile.BirthDate;
        //        }


        //        IFormFile file = updatedProfile.ImageFile;
        //        if (file != null)
        //        {
        //            string NewFileName = await Helpers.SaveFile("wwwroot/uploads", file);
        //            user.ImageUrl = NewFileName;
        //        }
        //        IdentityResult result = await _userManager.UpdateAsync(res);
        //        if (!result.Succeeded)
        //        {
        //            responseModel.StatusCode = 500;
        //            //responseModel.ErrorMessage = result.Errors.Select(e => e.Description).FirstOrDefault();
        //            responseModel.ErrorMessage = "User was not Updated";
        //            responseModel.Data = new DataModel { Data = "", Message = "" };
        //            return responseModel;
        //        }
        //        await _context.SaveChangesAsync();


        //        string _Email = user.Email;


        //        Profile_VM userProfile = await _context.AccProfiles.Where(x => x.UserId == uid).Select(x =>
        //        new Profile_VM
        //        {
        //            Email = x.Email ?? "",
        //            ImageUrl = x.ImageUrl != null ? $"{Request.Scheme}://{Request.Host}/Uploads/{x.ImageUrl}" : $"{Request.Scheme}://{Request.Host}/Uploads/user-placeholder.png",
        //            Role = x.Role.RoleName ?? "Not set",
        //            Id = x.Id,
        //            PhoneNumber = x.PhoneNumber ?? "Not Set",
        //            Name = x.Name ?? "Not Set",
        //            BirthDate = (DateTime)x.BirthDate,
        //            Gender = x.Gender.Name ?? "",
        //            GenderId = (int)x.GenderId,
        //            Token = "",

        //        }).FirstOrDefaultAsync();

        //        responseModel.StatusCode = 200;
        //        responseModel.ErrorMessage = "";
        //        responseModel.Data = new DataModel
        //        {
        //            Data = userProfile,
        //            Message = ""
        //        };
        //        return responseModel;

        //    }

        //    responseModel.StatusCode = 404;
        //    responseModel.ErrorMessage = "User Not Found";
        //    responseModel.Data = new DataModel { Data = "", Message = "" };
        //    return responseModel;
        //}

        //public async Task<ResponseModel> ConfirmEmail(string email, string token)
        //{
        //    ResponseModel responseModel = new ResponseModel();
        //    try
        //    {
        //        ApplicationUser user = await _userManager.FindByEmailAsync(email);
        //        IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
        //        if (result.Succeeded)
        //        {

        //            responseModel.StatusCode = 200;
        //            responseModel.ErrorMessage = "";
        //            responseModel.Data = new DataModel
        //            {
        //                Data = "Email Confirmed",
        //                Message = ""
        //            };
        //            return responseModel;
        //        }

        //        responseModel.StatusCode = 500;
        //        responseModel.ErrorMessage = result.Errors.Select(x => x.Description).FirstOrDefault();
        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        return responseModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        responseModel.StatusCode = 500;
        //        responseModel.ErrorMessage = ex.ToString();
        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        return responseModel;
        //        throw;
        //    }

        //}

        //public async Task<ResponseModel> ResendConfirmEmail(string email, HttpRequest Request)
        //{
        //    try
        //    {
        //        ApplicationUser user = await _userManager.FindByEmailAsync(email);

        //        ResponseModel responseModel = new ResponseModel();

        //        var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));

        //        string url = $"{Request.Scheme}://{Request.Host}/api/Accounts/ConfirmEmail?token=" + token + "&email=" + user.Email;
        //        await Helpers.SendEmailAsync(user.Email, "Confirm Email", "<h1>Follow the instructions to confirm your email</h1>" +
        //             $"<p> <a href='{url}'>Click here</a></p>");

        //        responseModel.StatusCode = 200;
        //        responseModel.ErrorMessage = "";
        //        responseModel.Data = new DataModel
        //        {
        //            Data = "Email Sent",
        //            Message = ""
        //        };
        //        return responseModel;


        //    }
        //    catch (Exception)
        //    {
        //        ResponseModel responseModel = new ResponseModel();


        //        responseModel.StatusCode = 500;
        //        responseModel.ErrorMessage = "";
        //        responseModel.Data = new DataModel
        //        {
        //            Data = "Some Thing Went Wrong",
        //            Message = ""
        //        };
        //        return responseModel;

        //        throw;
        //    }

        //}

        public async Task<ResponseModel> GenerateOtp(string phone)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                phone = Helpers.RemoveCountryCode(phone);

                var currphoneOtp = await _context.PhoneOtps.Where(x => x.PhoneNumber == phone).FirstOrDefaultAsync();
                if (currphoneOtp != null)
                {
                    var resend = await ResendOtp(phone);

                    responseModel.StatusCode = 200;
                    responseModel.ErrorMessage = "";
                    responseModel.Data = new DataModel
                    {
                        Data = "",
                        Message = "Otp has been Sent again to phone"
                    };
                    return responseModel;
                }
                var otp = Helpers.Generate_otp();

                PhoneOtp phoneOtp = new PhoneOtp()
                {
                    Otp = otp,
                    PhoneNumber = phone
                };

                await _context.PhoneOtps.AddAsync(phoneOtp);
                await _context.SaveChangesAsync();

                string content = $"your One-Time Password is : {otp}";

                Helpers.SendSMS(phone, content);

                responseModel.StatusCode = 200;
                responseModel.ErrorMessage = "";
                responseModel.Data = new DataModel
                {
                    Data = "",
                    Message = "Otp has been Sent to phone"
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

        public async Task<ResponseModel> ConfirmOtp(string otp, string phone)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                phone = Helpers.RemoveCountryCode(phone);

                var phoneOtp = await _context.PhoneOtps.Where(x => x.PhoneNumber == phone).FirstOrDefaultAsync();

                int dbotp = Convert.ToInt32(phoneOtp.Otp);
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
                    phoneOtp.Otp = newotp;
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

        //public async Task<AspNetUser> CreateProfile(EmailSignUp_VM model, string roleId)
        //{
        //    // create user profile and add role based on roleId
        //    AspNetUser newProfile = new AspNetUser();
        //    newProfile.Id = roleId;
        //    newProfile.Id = _context.AspNetUsers.Where(x => x.Email == model.Email).FirstOrDefault().Id;
        //    if (!string.IsNullOrEmpty(model.PhoneNumber))
        //    {
        //        newProfile.PhoneNumber = model.PhoneNumber;
        //    }
        //    if (!string.IsNullOrEmpty(model.Email))
        //    {
        //        newProfile.Email = model.Email;
        //    }

        //    if (!string.IsNullOrEmpty(model.Name))
        //    {
        //        newProfile.Name = model.Name;

        //    }
        //    if (model.Birthdate != default)
        //    {
        //        newProfile.BirthDate = model.Birthdate;
        //    }
        //    else
        //    {
        //        newProfile.BirthDate = new DateTime();
        //    }
        //    if (model.GenderId != default)
        //    {
        //        newProfile.GenderId = model.GenderId;
        //    }
        //    IFormFile file = model.ImageFile;
        //    if (file != null)
        //    {
        //        string NewFileName = await Helpers.SaveFile("wwwroot/uploads", file);

        //        newProfile.ImageUrl = NewFileName;
        //    }
        //    else
        //    {
        //        newProfile.ImageUrl = "user-placeholder.png";
        //    }

        //    newProfile.DateCreated = DateTime.UtcNow;
        //    newProfile.IsDeleted = false;


        //    // check if user sent profile picture
        //    await _context.AccProfiles.AddAsync(newProfile);
        //    await _context.SaveChangesAsync();
        //    return newProfile;
        //}


        public async Task<ResponseModel> ResendOtp(string phone)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {

                phone = Helpers.RemoveCountryCode(phone);


                var phoneOtp = await _context.PhoneOtps.Where(x => x.PhoneNumber == phone).FirstOrDefaultAsync();


                var otp = Helpers.Generate_otp();

                phoneOtp.Otp = otp;

                await _context.SaveChangesAsync();
                string content = $"your One-Time Password is : {otp}";
                Helpers.SendSMS(phone, content);

                responseModel.StatusCode = 200;
                responseModel.ErrorMessage = "";
                responseModel.Data = new DataModel
                {
                    Data = "",
                    Message = "Otp has been Resent to phone"
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


        //public async Task<ResponseModel> ConfirmAccount(string phonenumber)
        //{
        //    ResponseModel responseModel = new ResponseModel();
        //    try
        //    {
        //        phonenumber = Helpers.RemoveCountryCode(phonenumber);

        //        var aspuser = _context.AspNetUsers.Where(x => x.PhoneNumber == phonenumber).FirstOrDefault();

        //        if (aspuser == null)
        //        {
        //            responseModel.StatusCode = 404;
        //            responseModel.ErrorMessage = "User was not Found";
        //            responseModel.Data = new DataModel { Data = "", Message = "" };
        //            return responseModel;
        //        }
        //        else
        //        {
        //            aspuser.EmailConfirmed = true;
        //            aspuser.PhoneNumberConfirmed = true;
        //            await _context.SaveChangesAsync();




        //            responseModel.StatusCode = 200;
        //            responseModel.ErrorMessage = "";
        //            responseModel.Data = new DataModel { Data = true, Message = "" };
        //            return responseModel;
        //        }
        //    }
        //    catch (Exception ex)
        //    {



        //        responseModel.StatusCode = 500;
        //        responseModel.ErrorMessage = ex.ToString();
        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        return responseModel;
        //        throw;
        //    }

        //}

        //public async Task<bool> CheckIfVerified(string email)
        //{
        //    try
        //    {
        //        bool isVerified = false;
        //        if (string.IsNullOrEmpty(email))
        //        {
        //            return isVerified;
        //        }
        //        else
        //        {
        //            var user = await _userManager.FindByEmailAsync(email);
        //            if (user == null)
        //            {
        //                return isVerified;
        //            }
        //            isVerified = user.PhoneNumberConfirmed;
        //            return isVerified;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //        throw;
        //    }
        //}


    }
}
