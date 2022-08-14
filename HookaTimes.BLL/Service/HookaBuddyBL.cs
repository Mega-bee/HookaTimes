using AutoMapper;
using HookaTimes.BLL.Enums;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using HookaTimes.DAL;
using HookaTimes.DAL.HookaTimesModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class HookaBuddyBL : BaseBO, IHookaBuddyBL
    {
        public HookaBuddyBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<ResponseModel> GetBuddies(HttpRequest request, int userBuddyId, string uid)
        {
            ResponseModel responseModel = new ResponseModel();
            int buddyId = await _uow.BuddyRepository.GetAll(b => b.UserId == uid).Select(b => b.Id).FirstOrDefaultAsync();

            List<HookaBuddy_VM> buddies = await _uow.BuddyRepository.GetAll(x => x.IsDeleted == false && x.Id != buddyId).Select(x => new HookaBuddy_VM
            {
                About = x.About ?? "",
                Id = x.Id,
                IsAvailable = x.IsAvailable ?? false,
                Name = x.FirstName + " " + x.LastName,
                Image = $"{request.Scheme}://{request.Host}{x.Image}",
                HasPendingInvite = x.InvitationToBuddies.Where(i => i.IsDeleted == false && i.FromBuddyId == userBuddyId && i.InvitationStatusId == 1).Any(),
                //Rating = x.Ra
            }).ToListAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = buddies, Message = "" };
            return responseModel;

        }
        public async Task<ResponseModel> GetBuddy(int BuddyId, HttpRequest Request)
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
            userProfile.Addresses = currProfile.BuddyProfileAddresses.Where(x => x.IsDeleted == false).Select(x => new BuddyProfileAddressVM
            {
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Title = x.Title,
                Id = x.Id,
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
        public async Task<ResponseModel> InviteBuddy(int userBuddyId, SendInvitation_VM model)
        {
            ResponseModel responseModel = new ResponseModel();
            bool buddyExists = await _uow.BuddyRepository.CheckIfExists(x => x.Id == model.ToBuddyId);
            bool placeExists = await _uow.BuddyRepository.CheckIfExists(x => x.Id == model.PlaceId);
            DateTime invitationDateTime = Convert.ToDateTime(model.Date + " " + model.Time);
            if (!buddyExists)
            {
                responseModel.ErrorMessage = "Buddy not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            if (!placeExists)
            {
                responseModel.ErrorMessage = "Place not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            Invitation invitation = new Invitation()
            {
                FromBuddyId = userBuddyId,
                ToBuddyId = model.ToBuddyId,
                CreatedDate = DateTime.UtcNow,
                InvitationDate = invitationDateTime,
                IsDeleted = false,
                InvitationOptionId = model.OptionId,
                Description = model.Description,
                InvitationStatusId = 1,
                PlaceId = model.PlaceId,
            };
            await _uow.InvitationRepository.Create(invitation);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 201;
            responseModel.Data = new DataModel { Data = "", Message = "Invitation Sent Succesfully" };
            return responseModel;
        }
        public async Task<List<Buddy_VM>> GetBuddiesMVC(HttpRequest request, int userBuddyId, int take = 0, int filterBy = 0, int sortBy = 0)
        {
            var query = _uow.BuddyRepository.GetAll(x => x.IsDeleted == false && x.Id != userBuddyId);
            List<Buddy_VM> buddies = Array.Empty<Buddy_VM>().ToList();


            switch (filterBy)
            {
                case 1:
                    query = query.Where(x => x.IsAvailable == true);
                    break;
                default:
                    break;
            }

            switch (sortBy)
            {
                case 1:
                    query = query.OrderByDescending(x => x.Rating);
                    break;
                default:
                    break;
            }

            if (take > 0)
            {
                query = query.Take(take);
            }


            buddies = await query.Select(x => new Buddy_VM
            {
                Profession = x.Profession ?? "",
                Id = x.Id,
                Name = x.FirstName + " " + x.LastName,
                Image = x.Image,
                Rating = (float)(x.Rating ?? 0),
                Address = x.BuddyProfileAddresses.Where(a => a.IsDeleted == false).Select(a => a.Title).FirstOrDefault() ?? "",

            }).ToListAsync();

            return buddies;
        }
    }
}
