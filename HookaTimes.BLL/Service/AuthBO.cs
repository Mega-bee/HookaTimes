using AutoMapper;
using HookaTimes.BLL.Enums;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.Utilities.Mailkit;
using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using HookaTimes.DAL;
using HookaTimes.DAL.Data;
using HookaTimes.DAL.HookaTimesModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Ocsp;
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
        private readonly IEmailSender _emailSender;
        private readonly HookaDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthBO(IUnitOfWork unit, IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, HookaDbContext context, NotificationHelper notificationHelper, RoleManager<IdentityRole> roleManager, IEmailSender emailSender) : base(unit, mapper, notificationHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        #region API

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
                Data = new
                {
                    Token = JwtToken,
                    Name = buddy.FirstName
                },
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


        #region GetProfile
        public async Task<ResponseModel> GetProfile(int BuddyId, HttpRequest Request)
        {
            bool profileExist = await _uow.BuddyRepository.CheckIfExists(x => x.Id == BuddyId && x.IsDeleted == false);

            //AccProfile user = _context.AccProfiles.Include(x => x.Gender).Include(x => x.Role).Where(x => x.UserId == uid && x.IsDeleted == false).FirstOrDefault();
            ResponseModel responseModel = new ResponseModel();

            if (!profileExist)
            {
                responseModel.StatusCode = 404;
                responseModel.ErrorMessage = "User was not Found";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            BuddyProfile currProfile = await _uow.BuddyRepository.GetAllWithPredicateAndIncludes(x => x.Id == BuddyId && x.IsDeleted == false, x => x.User, x => x.BuddyProfileAddresses, x => x.BuddyProfileEducations, x => x.BuddyProfileExperiences).FirstOrDefaultAsync();


            Profile_VM userProfile = new Profile_VM();

            userProfile.ImageUrl = $"{Request.Scheme}://{Request.Host}{currProfile.Image}";
            userProfile.Name = currProfile.FirstName + " " + currProfile.LastName ?? "";
            userProfile.Email = currProfile.User.Email ?? "";
            userProfile.PhoneNumber = currProfile.User.PhoneNumber ?? "";
            userProfile.BirthDate = currProfile.DateOfBirth != default ? currProfile.DateOfBirth : new DateTime();
            userProfile.GenderId = currProfile.GenderId;
            userProfile.Gender = currProfile.GenderId != null ? Enum.GetName(typeof(GenderEnum), currProfile.GenderId) : "";
            userProfile.AboutMe = currProfile.About ?? "";
            userProfile.Hobbies = currProfile.Hobbies ?? "";
            userProfile.MaritalStatus = currProfile.MaritalStatus != null ? Enum.GetName(typeof(MaritalStatusEnum), currProfile.MaritalStatus) : "";
            userProfile.Height = currProfile.Height != default ? currProfile.Height : default;
            userProfile.Weight = currProfile.Weight != default ? currProfile.Weight : default;
            userProfile.BodyType = currProfile.BodyType != null ? Enum.GetName(typeof(BodyTypeEnum), currProfile.BodyType) : "";
            userProfile.Eyes = currProfile.Eyes != null ? Enum.GetName(typeof(EyeEnum), currProfile.Eyes) : "";
            userProfile.Hair = currProfile.Hair != null ? Enum.GetName(typeof(HairEnum), currProfile.Hair) : "";
            userProfile.SocialMediaLink1 = currProfile.SocialMediaLink1 ?? "";
            userProfile.SocialMediaLink2 = currProfile.SocialMediaLink2 ?? "";
            userProfile.SocialMediaLink3 = currProfile.SocialMediaLink3 ?? "";
            userProfile.Interests = currProfile.Interests ?? "";
            userProfile.Profession = currProfile.Profession ?? "";
            userProfile.FirstName = currProfile.FirstName ?? "";
            userProfile.LastName = currProfile.LastName ?? "";
            userProfile.HairId = currProfile.Hair;
            userProfile.MaritalStatusId = currProfile.MaritalStatus;
            userProfile.EyesId = currProfile.Eyes;
            userProfile.BodyTypeId = currProfile.BodyType;

            userProfile.IsAvailable = currProfile.IsAvailable;
            userProfile.Addresses = currProfile.BuddyProfileAddresses.Where(x => x.IsDeleted == false).Select(x => new BuddyProfileAddressVM
            {
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Title = x.Title,
                Id = x.Id



            }).ToList();
            userProfile.Education = currProfile.BuddyProfileEducations.Select(x => new BuddyProfileEducationVM
            {
                Degree = x.Degree,
                StudiedFrom = x.StudiedFrom,
                StudiedTo = x.StudiedTo,
                University = x.University,
                Id = x.Id

            }).ToList();
            userProfile.Experience = currProfile.BuddyProfileExperiences.Select(x => new BuddyProfileExperienceVM
            {
                Place = x.Place,
                Position = x.Position,
                WorkedFrom = x.WorkedFrom,
                WorkedTo = x.WorkedTo,
                Id = x.Id

            }).ToList();
            responseModel.StatusCode = 200;
            responseModel.ErrorMessage = "";
            responseModel.Data = new DataModel
            {
                Data = userProfile,
                Message = ""
            };
            return responseModel;
        }
        #endregion


        #region UpdateProfile
        public async Task<ResponseModel> CompleteProfile(CompleteProfile_VM model, int BuddyId, HttpRequest Request)
        {

            try
            {

                bool profileExist = await _uow.BuddyRepository.CheckIfExists(x => x.Id == BuddyId && x.IsDeleted == false);

                //AccProfile user = _context.AccProfiles.Include(x => x.Gender).Include(x => x.Role).Where(x => x.UserId == uid && x.IsDeleted == false).FirstOrDefault();
                ResponseModel responseModel = new ResponseModel();

                if (!profileExist)
                {
                    responseModel.StatusCode = 404;
                    responseModel.ErrorMessage = "User was not Found";
                    responseModel.Data = new DataModel { Data = "", Message = "" };
                    return responseModel;
                }

                BuddyProfile currProfile = await _uow.BuddyRepository.GetAllWithPredicateAndIncludes(x => x.Id == BuddyId && x.IsDeleted == false, x => x.User, y => y.BuddyProfileAddresses, y => y.BuddyProfileEducations, y => y.BuddyProfileExperiences).FirstOrDefaultAsync();

                currProfile.Profession = model.Profession;
                currProfile.Weight = model.Weight;
                currProfile.MaritalStatus = model.MaritalStatus;
                currProfile.About = model.AboutMe;
                currProfile.BodyType = model.BodyType;
                currProfile.CreatedDate = DateTime.UtcNow;
                currProfile.DateOfBirth = model.Birthdate != default ? model.Birthdate : model.Birthdate = new DateTime();
                currProfile.Eyes = model.Eyes;
                currProfile.FirstName = model.FirstName;
                currProfile.LastName = model.LastName;
                currProfile.GenderId = model.GenderId;
                currProfile.Hair = model.Hair;
                currProfile.Height = model.Height;
                currProfile.Hobbies = model.Hobbies;
                currProfile.Interests = model.Interests;
                currProfile.SocialMediaLink1 = model.SocialMediaLink1;
                currProfile.SocialMediaLink2 = model.SocialMediaLink2;
                currProfile.SocialMediaLink3 = model.SocialMediaLink3;



                IFormFile file = model.ImageFile;
                if (file != null)
                {
                    string NewFileName = await Helpers.SaveFile("wwwroot/Images/Buddies", file);

                    currProfile.Image = NewFileName;
                }

                await _context.SaveChangesAsync();


                Profile_VM userProfile = new Profile_VM();

                userProfile.ImageUrl = $"{Request.Scheme}://{Request.Host}{currProfile.Image}";
                userProfile.Name = currProfile.FirstName + " " + currProfile.LastName ?? "";
                userProfile.Email = currProfile.User.Email ?? "";
                userProfile.PhoneNumber = currProfile.User.PhoneNumber ?? "";
                userProfile.BirthDate = currProfile.DateOfBirth != default ? currProfile.DateOfBirth : new DateTime();
                userProfile.GenderId = currProfile.GenderId;
                userProfile.Gender = currProfile.GenderId != null ? Enum.GetName(typeof(GenderEnum), currProfile.GenderId) : "";
                userProfile.AboutMe = currProfile.About ?? "";
                userProfile.Hobbies = currProfile.Hobbies ?? "";
                userProfile.MaritalStatus = currProfile.MaritalStatus != null ? Enum.GetName(typeof(MaritalStatusEnum), currProfile.MaritalStatus) : "";
                userProfile.Height = currProfile.Height != default ? currProfile.Height : default;
                userProfile.Weight = currProfile.Weight != default ? currProfile.Weight : default;
                userProfile.BodyType = currProfile.BodyType != null ? Enum.GetName(typeof(BodyTypeEnum), currProfile.BodyType) : "";
                userProfile.Eyes = currProfile.Eyes != null ? Enum.GetName(typeof(EyeEnum), currProfile.Eyes) : "";
                userProfile.Hair = currProfile.Hair != null ? Enum.GetName(typeof(HairEnum), currProfile.Hair) : "";
                userProfile.SocialMediaLink1 = currProfile.SocialMediaLink1 ?? "";
                userProfile.SocialMediaLink2 = currProfile.SocialMediaLink2 ?? "";
                userProfile.SocialMediaLink3 = currProfile.SocialMediaLink3 ?? "";
                userProfile.Interests = currProfile.Interests ?? "";
                userProfile.Profession = currProfile.Profession ?? "";
                userProfile.FirstName = currProfile.FirstName ?? "";
                userProfile.LastName = currProfile.LastName ?? "";
                userProfile.HairId = currProfile.Hair;
                userProfile.MaritalStatusId = currProfile.MaritalStatus;
                userProfile.EyesId = currProfile.Eyes;
                userProfile.BodyTypeId = currProfile.BodyType;
                userProfile.IsAvailable = currProfile.IsAvailable;
                userProfile.Addresses = await _uow.BuddyProfileAddressRepository.GetAll(x => x.IsDeleted == false).Select(x => new BuddyProfileAddressVM
                {
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Title = x.Title,
                    Id = x.Id,
                }).ToListAsync();
                userProfile.Education = await _uow.BuddyProfileEducationRepository.GetAll().Select(x => new BuddyProfileEducationVM
                {
                    Degree = x.Degree,
                    StudiedFrom = x.StudiedFrom,
                    StudiedTo = x.StudiedTo,
                    University = x.University,
                    Id = x.Id

                }).ToListAsync();
                userProfile.Experience = await _uow.BuddyProfileExperienceRepository.GetAll().Select(x => new BuddyProfileExperienceVM
                {
                    Place = x.Place,
                    Position = x.Position,
                    WorkedFrom = x.WorkedFrom,
                    WorkedTo = x.WorkedTo,
                    Id = x.Id

                }).ToListAsync();

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

        public async Task<ResponseModel> AddAddress(BuddyProfileAddressPutVM Address, int BuddyId)
        {

            bool profileExist = await _uow.BuddyRepository.CheckIfExists(x => x.Id == BuddyId && x.IsDeleted == false);

            //AccProfile user = _context.AccProfiles.Include(x => x.Gender).Include(x => x.Role).Where(x => x.UserId == uid && x.IsDeleted == false).FirstOrDefault();
            ResponseModel responseModel = new ResponseModel();

            if (!profileExist)
            {
                responseModel.StatusCode = 404;
                responseModel.ErrorMessage = "User was not Found";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            if (Address != null)
            {


                BuddyProfileAddress newedu = new BuddyProfileAddress();

                newedu.BuddyProfileId = BuddyId;
                newedu.Latitude = Address.Latitude;
                newedu.Longitude = Address.Longitude;
                newedu.Title = Address.Title;
                newedu.IsDeleted = false;
                newedu.CreatedDate = DateTime.UtcNow;
                newedu.Apartment = Address.Appartment;
                newedu.Street = Address.Street;
                newedu.Building = Address.Building;
                newedu.City = Address.City;
                await _uow.BuddyProfileAddressRepository.Create(newedu);


                BuddyProfileAddressPutVM add = new BuddyProfileAddressPutVM()
                {
                    Appartment = newedu.Apartment,
                    Building = newedu.Building,
                    City = newedu.City,
                    Street = newedu.Street,
                    IsDeleted = (bool)newedu.IsDeleted,
                    Latitude = newedu.Latitude,
                    Longitude = newedu.Longitude,
                    Title = newedu.Title,
                    Id = newedu.Id,


                };


                responseModel.StatusCode = 201;
                responseModel.ErrorMessage = "";
                responseModel.Data = new DataModel { Data = add, Message = "" };
                return responseModel;
            }

            responseModel.StatusCode = 400;
            responseModel.ErrorMessage = "No address was received";
            responseModel.Data = new DataModel { Data = "", Message = "" };
            return responseModel;


        }


        public async Task<ResponseModel> DeleteAddress(int AddressId, int BuddyId)
        {

            bool profileExist = await _uow.BuddyRepository.CheckIfExists(x => x.Id == BuddyId && x.IsDeleted == false);

            //AccProfile user = _context.AccProfiles.Include(x => x.Gender).Include(x => x.Role).Where(x => x.UserId == uid && x.IsDeleted == false).FirstOrDefault();
            ResponseModel responseModel = new ResponseModel();

            if (!profileExist)
            {
                responseModel.StatusCode = 404;
                responseModel.ErrorMessage = "User was not Found";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            if (AddressId != default)
            {
                BuddyProfileAddress address = await _uow.BuddyProfileAddressRepository.GetFirst(x => x.Id == AddressId);
                address.IsDeleted = true;
                await _uow.BuddyProfileAddressRepository.Update(address);

                responseModel.StatusCode = 201;
                responseModel.ErrorMessage = "";
                responseModel.Data = new DataModel { Data = "", Message = "Address Deleted Successfully" };
                return responseModel;
            }

            responseModel.StatusCode = 400;
            responseModel.ErrorMessage = "Address was not found";
            responseModel.Data = new DataModel { Data = "", Message = "" };
            return responseModel;


        }



        public async Task<ResponseModel> AddEducation(BuddyProfileEducationPutVM education, int BuddyId)
        {

            bool profileExist = await _uow.BuddyRepository.CheckIfExists(x => x.Id == BuddyId && x.IsDeleted == false);

            ResponseModel responseModel = new ResponseModel();

            if (!profileExist)
            {
                responseModel.StatusCode = 404;
                responseModel.ErrorMessage = "User was not Found";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            if (education != null)
            {
                BuddyProfileEducation newedu = new BuddyProfileEducation()
                {
                    BuddyProfileId = BuddyId,
                    Degree = education.Degree,
                    StudiedFrom = education.StudiedFrom,
                    StudiedTo = education.StudiedTo,
                    University = education.University,
                    CreatedDate = DateTime.UtcNow

                };
                await _uow.BuddyProfileEducationRepository.Create(newedu);

                BuddyProfileEducationPutVM edu = new BuddyProfileEducationPutVM()
                {
                    Id = newedu.Id,
                    IsDeleted = education.IsDeleted,
                    Degree = newedu.Degree,
                    StudiedFrom = newedu.StudiedFrom,
                    StudiedTo = newedu.StudiedTo,
                    University = newedu.University,
                };
                responseModel.StatusCode = 201;
                responseModel.ErrorMessage = "";
                responseModel.Data = new DataModel { Data = edu, Message = "" };
                return responseModel;
            }


            responseModel.StatusCode = 400;
            responseModel.ErrorMessage = "No Education was received";
            responseModel.Data = new DataModel { Data = "", Message = "" };
            return responseModel;

        }

        public async Task<ResponseModel> DeleteEducation(int EducationId)
        {

            bool eduExist = await _uow.BuddyProfileEducationRepository.CheckIfExists(x => x.Id == EducationId);

            ResponseModel responseModel = new ResponseModel();

            if (!eduExist)
            {
                responseModel.StatusCode = 404;
                responseModel.ErrorMessage = "Education was not Found";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            await _uow.BuddyProfileEducationRepository.Delete(EducationId);


            responseModel.StatusCode = 200;
            responseModel.ErrorMessage = "Education Deleted";
            responseModel.Data = new DataModel { Data = "", Message = "" };
            return responseModel;


        }


        public async Task<ResponseModel> AddExperience(BuddyProfileExperiencePutVM exp, int BuddyId)
        {

            bool profileExist = await _uow.BuddyRepository.CheckIfExists(x => x.Id == BuddyId && x.IsDeleted == false);

            ResponseModel responseModel = new ResponseModel();

            if (!profileExist)
            {
                responseModel.StatusCode = 404;
                responseModel.ErrorMessage = "User was not Found";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            if (exp != null)
            {




                BuddyProfileExperience newedu = new BuddyProfileExperience()
                {
                    BuddyProfileId = BuddyId,
                    Place = exp.Place,
                    Position = exp.Position,
                    WorkedFrom = exp.WorkedFrom,
                    WorkedTo = exp.WorkedTo,
                    CreatedDate = DateTime.UtcNow


                };
                await _uow.BuddyProfileExperienceRepository.Create(newedu);

                BuddyProfileExperiencePutVM expp = new BuddyProfileExperiencePutVM()
                {
                    Id = newedu.Id,
                    IsDeleted = exp.IsDeleted,
                    Place = newedu.Place,
                    Position = newedu.Position,
                    WorkedFrom = newedu.WorkedFrom,
                    WorkedTo = newedu.WorkedTo,


                };

                responseModel.StatusCode = 201;
                responseModel.ErrorMessage = "";
                responseModel.Data = new DataModel { Data = expp, Message = "" };
                return responseModel;
            }


            responseModel.StatusCode = 400;
            responseModel.ErrorMessage = "No Experience was received";
            responseModel.Data = new DataModel { Data = "", Message = "" };
            return responseModel;

        }

        public async Task<ResponseModel> DeleteExperience(int ExpId)
        {

            bool ExpExist = await _uow.BuddyProfileExperienceRepository.CheckIfExists(x => x.Id == ExpId);

            ResponseModel responseModel = new ResponseModel();

            if (!ExpExist)
            {
                responseModel.StatusCode = 404;
                responseModel.ErrorMessage = "Experience was not Found";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            await _uow.BuddyProfileExperienceRepository.Delete(ExpId);


            responseModel.StatusCode = 200;
            responseModel.ErrorMessage = "Experience Deleted";
            responseModel.Data = new DataModel { Data = "", Message = "" };
            return responseModel;

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
                Data = new
                {
                    //Token = JwtToken,
                    Name = buddy.FirstName
                },
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
            newProfile.Image = "/Images/Buddies/user-placeholder.png";
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

            bool isEmailSent = await _emailSender.SendEmailAsync(identifier, "Reset Password", content);
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

                    //responseModel.StatusCode = 200;
                    //responseModel.ErrorMessage = "";
                    //responseModel.Data = new DataModel
                    //{
                    //    Data = "",
                    //    Message = "Otp has been Sent again to Email"
                    //};
                    return resend;
                }
                var otp = Helpers.Generate_otp();

                EmailOtp emailOtp = new EmailOtp()
                {
                    Email = Email,
                    Otp = otp,
                };

                await _context.EmailOtps.AddAsync(emailOtp);
                await _context.SaveChangesAsync();

                //string content = $"Deare Hooka Buddy \n Your Verification Pin is : {otp} \n Thank you for choosing Hooka Times";
                

                //Helpers.SendSMS(phone, content);
                //bool EmailSent = await Tools.SendEmailAsync(Email, "Hooka OTP", content);
               bool isEmailSent =  await _emailSender.SendEmailAsync(Email, "Hooka OTP", otp);
                //if (!EmailSent)
                //{
                //    responseModel.StatusCode = 400;
                //    responseModel.ErrorMessage = "";
                //    responseModel.Data = new DataModel
                //    {
                //        Data = "",
                //        Message = "Email Was Not Sent"
                //    };
                //    return responseModel;
                //}
                if(!isEmailSent)
                {
                    responseModel.StatusCode = 500;
                    responseModel.ErrorMessage = "Failed to send email";
                    responseModel.Data = new DataModel
                    {
                        Data = "",
                        Message = ""
                    };
                    return responseModel;
                }
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
                //string content = $"Dear Hooka Buddy \n Your Verification Pin is : {otp} \n Thank you for choosing Hooka Times";

                //Helpers.SendSMS(phone, content);
                bool isEmailSent = await _emailSender.SendEmailAsync(Email, "Hooka OTP", otp);
                //bool EmailSent = await Tools.SendEmailAsync(Email, "Hooka OTP", content);
                //if (!EmailSent)
                //{
                //    responseModel.StatusCode = 400;
                //    responseModel.ErrorMessage = "";
                //    responseModel.Data = new DataModel
                //    {
                //        Data = "",
                //        Message = "Email Was Not Sent"
                //    };
                //    return responseModel;
                //}
                if (!isEmailSent)
                {
                    responseModel.StatusCode = 500;
                    responseModel.ErrorMessage = "Failed to send email";
                    responseModel.Data = new DataModel
                    {
                        Data = "",
                        Message = ""
                    };
                    return responseModel;
                }
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


        #region Available Toggle
        public async Task<ResponseModel> IsAvailableToggle(int buddyId)
        {
            var responseModel = new ResponseModel();
            BuddyProfile buddy = await _uow.BuddyRepository.GetFirst(x => x.Id == buddyId && x.IsDeleted == false);
            if (buddy == null)
            {
                responseModel.ErrorMessage = "User was not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
            }

            buddy.IsAvailable = !buddy.IsAvailable;

            await _uow.BuddyRepository.Update(buddy);

            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = "", Message = $"{((bool)buddy.IsAvailable ? "Available" : "Not Available")}" };
            return responseModel;
        }

        #endregion

        #endregion

        #region MVC

        #region SignIn
        public async Task<ClaimsIdentity> EmailSignInMVC(EmailSignInMVC_VM model, string wishlistSessionId, string cartSessionId)
        {

            ApplicationUser res = await _userManager.FindByEmailAsync(model.Email);
            if (res == null)
            {
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(res, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return null;
            }



            BuddyProfile buddy = await _uow.BuddyRepository.GetFirst(x => x.UserId == res.Id);
            if (!string.IsNullOrEmpty(cartSessionId))
            {
                await MoveCartFromCookiesToUser(buddy.Id, cartSessionId);
            }
            if (!string.IsNullOrEmpty(wishlistSessionId))
            {
                await MoveWishlistFromCookiesToUser(buddy.Id, wishlistSessionId);
            }
            var roles = await _userManager.GetRolesAsync(res);

            var claims = Tools.GenerateClaimsMVC(res, roles, buddy);

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return identity;

            //string JwtToken = Tools.GenerateJWT(claims);

            /////// adding claims to db
            //var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(res);

            //var claimResult = claimsPrincipal.Claims.ToList();

            //if (claims != null && claims.Count > 0)
            //{
            //    await _userManager.AddClaimsAsync(res, claims.ToList());

            //}

            //await _signInManager.RefreshSignInAsync(res);


            //return identity;





        }
        public async Task MoveCartFromCookiesToUser(int buddyId, string cartSessionId)
        {
            Cart cartItem = new Cart();
            List<Cart> userCartItems = await _uow.CartRepository.GetAllWithTracking(x => x.BuddyId == buddyId).ToListAsync();
            List<VirtualCart> sessionCartItems = await _uow.VirtualCartRepository.GetAll(x => x.SessionCartId == cartSessionId).ToListAsync();

            if (sessionCartItems.Count > 0)
            {
                foreach (var item in sessionCartItems)
                {
                    if (userCartItems.Count > 0)
                    {
                        cartItem = userCartItems.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                        if (cartItem != null)
                        {
                            cartItem.Quantity += item.Quantity;
                            cartItem.UpdatedDate = DateTime.UtcNow;
                        }
                        else
                        {
                            cartItem = new Cart()
                            {
                                Quantity = item.Quantity,
                                ProductId = item.ProductId,
                                BuddyId = buddyId,
                                CreatedDate = DateTime.UtcNow
                            };
                            await _uow.CartRepository.Add(cartItem);
                        }
                    }
                    else
                    {
                        cartItem = new Cart()
                        {
                            Quantity = item.Quantity,
                            ProductId = item.ProductId,
                            BuddyId = buddyId,
                            CreatedDate = DateTime.UtcNow
                        };
                        await _uow.CartRepository.Add(cartItem);
                    }



                }
                await _uow.SaveAsync();
                await _uow.VirtualCartRepository.DeleteRange(sessionCartItems);
            }

        }

        public async Task MoveWishlistFromCookiesToUser(int buddyId, string wishlistSessionId)
        {
            Wishlist cartItem = new Wishlist();
            List<Wishlist> userCartItems = await _uow.WishlistRepository.GetAllWithTracking(x => x.BuddyId == buddyId).ToListAsync();
            List<VirtualWishlist> sessionCartItems = await _uow.VirtualWishlistRepository.GetAll(x => x.WishlistSessionId == wishlistSessionId).ToListAsync();

            if (sessionCartItems.Count > 0)
            {
                foreach (var item in sessionCartItems)
                {
                    if (userCartItems.Count > 0)
                    {
                        cartItem = userCartItems.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                        if (cartItem == null)
                        {
                            cartItem = new Wishlist()
                            {

                                ProductId = item.ProductId,
                                BuddyId = buddyId,
                                CreatedDate = DateTime.UtcNow
                            };
                            await _uow.WishlistRepository.Add(cartItem);
                        }

                    }
                    else
                    {

                        cartItem = new Wishlist()
                        {

                            ProductId = item.ProductId,
                            BuddyId = buddyId,
                            CreatedDate = DateTime.UtcNow
                        };
                        await _uow.WishlistRepository.Add(cartItem);
                    }



                }
                await _uow.SaveAsync();
                await _uow.VirtualWishlistRepository.DeleteRange(sessionCartItems);
            }

        }
        #endregion


        #region Buddy
        public async Task<int> GetBuddyById(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return 0;
            }
            int buddyId = await _uow.BuddyRepository.GetAll(x => x.UserId == UserId).Select(x => x.Id).FirstOrDefaultAsync();

            return buddyId;
        }


        public async Task<NavBuddy_VM> GetNavBuddyProfile(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return null;
            }
            NavBuddy_VM Buddy = await _uow.BuddyRepository.GetAll(x => x.UserId == UserId).Select(x => new NavBuddy_VM
            {
                Email = x.User.Email,
                FirstName = x.FirstName,
                Image = x.Image
            }).FirstOrDefaultAsync();

            return Buddy;
        }
        #endregion

        #region SignUp

        public async Task<IdentityResult> SignUpWithEmailMVC(EmailSignUpMVC_VM model)
        {
            await CheckRoles();

            ApplicationUser oldUser = await _userManager.FindByEmailAsync(model.Email);
            if (oldUser != null)
            {
                return null;
            }


            IdentityResult res = await CreateUserMVC(model);

            if (!res.Succeeded)
            {
                return null;
            }



            return res;

        }



        public async Task<IdentityResult> CreateUserMVC(EmailSignUpMVC_VM model)
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



        public async Task<BuddyProfile> CreateBuddyProfileMVC(EmailSignUpMVC_VM model, string cartSessionId, string wishlistSessionId)
        {
            // create user profile and add role based on roleId
            BuddyProfile newProfile = new BuddyProfile();
            newProfile.UserId = _context.AspNetUsers.Where(x => x.Email == model.Email).FirstOrDefault().Id;
            newProfile.IsDeleted = false;
            newProfile.CreatedDate = DateTime.UtcNow;
            newProfile.FirstName = model.FirstName;
            newProfile.LastName = model.LastName;
            newProfile.IsAvailable = true;
            newProfile.Image = "Images/Buddies/user-placeholder.png";
            //newProfile.GenderId = 1;
            // add the characteristics to the BuddyProfiles
            await _context.BuddyProfiles.AddAsync(newProfile);
            await _context.SaveChangesAsync();
            if (!string.IsNullOrEmpty(cartSessionId))
            {
                await MoveCartFromCookiesToUser(newProfile.Id, cartSessionId);
            }
            if (!string.IsNullOrEmpty(wishlistSessionId))
            {
                await MoveWishlistFromCookiesToUser(newProfile.Id, wishlistSessionId);
            }
            return newProfile;
        }



        #endregion

        #region OrderHistory
        public async Task<List<OrderHistoryMVC_VM>> GetOrderHistoryMVC(int BuddyId)
        {

            List<OrderHistoryMVC_VM> orderHistory = await _uow.OrderRepository.GetAll(x => x.BuddyId == BuddyId).Select(x => new OrderHistoryMVC_VM
            {
                Id = x.Id,
                Date = x.CreatedDate.Value.ToString("dd MMMM, yyyy"),
                Status = x.OrderStatus.Title,
                Total = x.Total.Value.ToString("0.##"),

            }).ToListAsync();

            return orderHistory;
        }
        #endregion

        public async Task<List<BuddyProfileAddressVM>> GetUserAddresses(int userBuddyId)
        {
            List<BuddyProfileAddressVM> addresses = await _uow.BuddyProfileAddressRepository.GetAll(x => x.IsDeleted == false && x.BuddyProfileId == userBuddyId).Select(x => new BuddyProfileAddressVM
            {
                Id = x.Id,
                Title = x.Title
            }).ToListAsync();
            return addresses;
        }


        #endregion


    }
}
