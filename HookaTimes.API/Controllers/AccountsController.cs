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

        //[HttpGet]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        //public async Task<IActionResult> GetUserProfile()
        //{

        //    string uid = User.Claims.Where(x => x.Type == "UID").FirstOrDefault().Value;
        //    ResponseModel getUserProfile = await _auth.GetUserProfile(uid, Request);
        //    return Ok(getUserProfile);


        //}

        //[HttpPost]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        //public async Task<IActionResult> ResetPassword([FromForm] ResetPassword_VM model)
        //{

        //    string uid = User.Claims.Where(x => x.Type == "UID").FirstOrDefault().Value;
        //    ResponseModel resetPassword = await _auth.ResetPassword(model, uid);
        //    return Ok(resetPassword);

        //}



        //[HttpPut]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        //public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfile_VM model)
        //{
        //    string uid = User.Claims.Where(x => x.Type == "UID").FirstOrDefault().Value;
        //    ResponseModel updateProfile = await _auth.UpdateProfile(model, uid, Request);
        //    return Ok(updateProfile);

        //}







    }


}
