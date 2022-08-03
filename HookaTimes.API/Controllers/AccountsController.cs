using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Service;
using HookaTimes.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HookaTimes.API.Controllers
{

    public class AccountsController : APIBaseController
    {
        private readonly IAuthBO _auth;

        public AccountsController(IAuthBO auth)
        {
            _auth = auth;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromForm] EmailSignIn_VM model)
        {
            ResponseModel resp = await _auth.EmailSignIn(model);
            return Ok(resp);
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword([FromForm] string identifier)
        {
            ResponseModel responseModel = new ResponseModel();
            ResponseModel forgetPassword = await _auth.ForgetPassword(identifier, Request);
            return Ok(forgetPassword);

        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm] EmailSignUp_VM model)
        {

            ResponseModel signup = await _auth.SignUpWithEmail(model, Request);
            return Ok(signup);

        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut]
        public async Task<IActionResult> RefreshFcmToken([FromForm] string token)
        {
            string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _auth.RefreshFcmToken(uid, token));
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> CompleteProfile([FromForm] CompleteProfile_VM model)
        {
            string uid = User.Claims.Where(x => x.Type == "UID").FirstOrDefault().Value;
            ResponseModel completeProfile = await _auth.CompleteProfile(model, uid, Request);
            return Ok(completeProfile);

        }


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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateOtp([FromForm] string phone)
        {

            ResponseModel GenerateOtp = await _auth.GenerateOtp(phone);
            return Ok(GenerateOtp);


        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmOtp([FromForm] string otp, [FromForm] string phone)
        {

            ResponseModel ConfirmOtp = await _auth.ConfirmOtp(otp, phone);
            return Ok(ConfirmOtp);

        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResendOtp([FromForm] string phone)
        {
            ResponseModel ResendOtp = await _auth.ResendOtp(phone);
            return Ok(ResendOtp);
        }



    }


}
