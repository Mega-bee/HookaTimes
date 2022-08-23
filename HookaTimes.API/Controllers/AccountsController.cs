using HookaTimes.BLL.IServices;
using HookaTimes.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HookaTimes.API.Controllers
{

    public class AccountsController : APIBaseController
    {
        private readonly IAuthBO _auth;

        public AccountsController(IAuthBO auth)
        {
            _auth = auth;
        }

        #region SignIn
        [HttpPost]
        public async Task<IActionResult> SignIn([FromForm] EmailSignIn_VM model)
        {
            ResponseModel resp = await _auth.EmailSignIn(model);
            return Ok(resp);
        }

        #endregion

        #region ForgetPassword
        [HttpPost]
        public async Task<IActionResult> ForgetPassword([FromForm] string identifier)
        {
            ResponseModel responseModel = new ResponseModel();
            ResponseModel forgetPassword = await _auth.SendChangePasswordToken(identifier);
            return Ok(forgetPassword);

        }

        [HttpPost]

        public async Task<IActionResult> ConsumeChangePasswordToken([FromForm] ConsumeChangePasswordToken_VM model)
        {
            ResponseModel responseModel = new ResponseModel();
            ResponseModel ConsumeChangePasswordToken = await _auth.ConsumeChangePasswordToken(model);
            return Ok(ConsumeChangePasswordToken);
        }
        #endregion

        #region SignUp
        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm] EmailSignUp_VM model)
        {

            ResponseModel signup = await _auth.SignUpWithEmail(model, Request);
            return Ok(signup);

        }

        #endregion

        #region Refrech FCM
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut]
        public async Task<IActionResult> RefreshFcmToken([FromForm] string token)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _auth.RefreshFcmToken(uid, token));
        }
        #endregion

        #region UpdateProfile
        [HttpPut]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> UpdateProfile([FromForm] CompleteProfile_VM model)
        {
            //string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);

            ResponseModel completeProfile = await _auth.CompleteProfile(model, userBuddyId, Request);
            return Ok(completeProfile);

        }
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AddAddress([FromForm] BuddyProfileAddressPutVM model)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            ResponseModel AddAddress = await _auth.AddAddress(model, userBuddyId);
            return Ok(AddAddress);
        }
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut]
        public async Task<IActionResult> DeleteAddress([FromForm] int AddressId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            ResponseModel resp = await _auth.DeleteAddress(AddressId, userBuddyId);
            return Ok(resp);
        }
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost]

        public async Task<IActionResult> AddEducation([FromForm] BuddyProfileEducationPutVM model)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            ResponseModel AddAddress = await _auth.AddEducation(model, userBuddyId);
            return Ok(AddAddress);
        }
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpDelete]
        public async Task<IActionResult> DeleteEducation([FromForm] int EducationId)
        {
            ResponseModel resp = await _auth.DeleteEducation(EducationId);
            return Ok(resp);
        }


        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost]

        public async Task<IActionResult> AddExperience([FromForm] BuddyProfileExperiencePutVM model)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            ResponseModel AddAddress = await _auth.AddExperience(model, userBuddyId);
            return Ok(AddAddress);
        }


        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpDelete]
        public async Task<IActionResult> DeleteExperience([FromForm] int DeleteExperience)
        {
            ResponseModel resp = await _auth.DeleteExperience(DeleteExperience);
            return Ok(resp);
        }

        #endregion

        #region OTP
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateOtp([FromForm] string Email)
        {

            ResponseModel GenerateOtp = await _auth.GenerateOtp(Email);
            return Ok(GenerateOtp);


        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmOtp([FromForm] string otp, [FromForm] string Email)
        {

            ResponseModel ConfirmOtp = await _auth.ConfirmOtp(otp, Email);
            return Ok(ConfirmOtp);

        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResendOtp([FromForm] string Email)
        {
            ResponseModel ResendOtp = await _auth.ResendOtp(Email);
            return Ok(ResendOtp);
        }
        #endregion

        #region GetProfile
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> GetProfile()
        {
            //string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);

            ResponseModel buddy = await _auth.GetProfile(userBuddyId, Request);
            return Ok(buddy);

        }
        #endregion

        #region Available Toggle
        [HttpPut]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> IsAvailableToggle()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _auth.IsAvailableToggle(userBuddyId));
        }
        #endregion

        #region Account Settings
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> GetAccountSettings()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID").Value);
            return Ok(await _auth.GetAccountSettings(userBuddyId));
        }
        #endregion







    }


}
