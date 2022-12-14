using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using HookaTimes.DAL.HookaTimesModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
//using HookaTimes.DAL.Models;

namespace HookaTimes.BLL.IServices
{
    public interface IAuthBO
    {
        //Task<CheckifExist_VM> CheckIfUserExists(string email, string phone);

        //Task<bool> CheckIfVerified(string email);

        Task<ResponseModel> CompleteProfile(CompleteProfile_VM model, int BuddyId, HttpRequest Request);

        Task<ResponseModel> DeleteAddress(int AddressId, int BuddyId);
        Task<ResponseModel> AddAddress(BuddyProfileAddressPutVM Address, int BuddyId);

        Task<ResponseModel> AddEducation(BuddyProfileEducationPutVM education, int BuddyId);
        Task<ResponseModel> DeleteEducation(int EducationId);
        Task<ResponseModel> AddExperience(BuddyProfileExperiencePutVM exp, int BuddyId);
        Task<ResponseModel> DeleteExperience(int ExpId);

        //Task<ResponseModel> ConfirmAccount(string phonenumber);

        //Task<ResponseModel> ConfirmEmail(string email, string token);
        Task<ResponseModel> GetProfile(int BuddyId, HttpRequest Request);
        Task<ResponseModel> ConfirmOtp(string otp, string phone);

        //Task<AspNetUser> CreateProfile(EmailSignUp_VM model, int roleId);

        Task<IdentityResult> CreateUser(EmailSignUp_VM model);

        Task<ResponseModel> EmailSignIn(EmailSignIn_VM model);

        //Task<ResponseModel> EmailSignIn(EmailSignIn_VM model, HttpRequest Request);

        //Task<ResponseModel> SendChangePasswordToken(string identifier);

        //List<Claim> GenerateClaims(ApplicationUser res, AspNetRole role);

        string GenerateJWT(List<Claim> claims);


        Task<ResponseModel> GenerateOtp(string phone);
        Task<ResponseModel> IsAvailableToggle(int buddyId);

        //Task<ResponseModel> GetUserProfile(string uid, HttpRequest Request);

        //Task<ResponseModel> ResendConfirmEmail(string email, HttpRequest Request);
        Task<ResponseModel> RefreshFcmToken(string uid, string token);
        Task<ResponseModel> ResendOtp(string phone);

        Task<ResponseModel> ResetPassword(ResetPassword_VM model, string uid);

        //Task<ResponseModel> ResetPasswordFromEmail(ResetPasswordFromEmail_VM model);

        Task<ResponseModel> SignUpWithEmail(EmailSignUp_VM model, HttpRequest Request);

        //Task<ResponseModel> ConsumeChangePasswordToken(ConsumeChangePasswordToken_VM model);

        //Task<ResponseModel> UpdateProfile(UpdateProfile_VM updatedProfile, string uid, HttpRequest Request);




        Task<ResponseModel> ForgetPassword(string identifier, HttpRequest Request);
        Task<ResponseModel> ResetPasswordFromEmail(ResetPasswordFromEmail_VM model);


        ///////////////////////////////////////////////MVC//////////////////////////////////////////
        //Task<bool> EmailSignInMVC(EmailSignInMVC_VM model);

        Task<ClaimsIdentity> EmailSignInMVC(EmailSignInMVC_VM model, string wishlistSessionId, string cartSessionId);

        Task<int> GetBuddyById(string UserId);

        Task<NavBuddy_VM> GetNavBuddyProfile(string UserId);

        Task<IdentityResult> SignUpWithEmailMVC(EmailSignUpMVC_VM model);
        Task<BuddyProfile> CreateBuddyProfileMVC(EmailSignUpMVC_VM model, string cartSessionId, string wishlistSessionId);
        Task<List<OrderHistoryMVC_VM>> GetOrderHistoryMVC(int BuddyId);

        Task<CompleteProfileMVC_VM> GetProfileMVC(int BuddyId);

        Task<bool> CompleteProfileMVC(CompleteProfileMVC_VM model, int BuddyId);

        Task<ResponseModel> GetUserAddresses(int userBuddyId);
        //Task<ResponseModel> GetAccountSettings(int userBuddyId);
        Task<ResponseModel> ChangePassword(ResetPassword_VM model, string email);
    }
}