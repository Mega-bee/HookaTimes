using AutoMapper;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.DAL;
using HookaTimes.DAL.HookaTimesModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class HookaBuddyBL : BaseBO, IHookaBuddyBL
    {
        public HookaBuddyBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<ResponseModel> GetBuddies(HttpRequest request, string uid)
        {
            ResponseModel responseModel = new ResponseModel();
            int buddyId = await _uow.BuddyRepository.GetAll(b => b.UserId == uid).Select(b => b.Id).FirstOrDefaultAsync();

            List<HookaBuddy_VM> buddies = await _uow.BuddyRepository.GetAll(x => x.IsDeleted == false && x.Id != buddyId).Select(x => new HookaBuddy_VM
            {
                About = x.About ?? "",
                Id = x.Id,
                IsAvailable = x.IsAvailable ?? false,
                Name = x.FirstName + " " + x.LastName,
                Image = $"{request.Scheme}://{request.Host}/Images/Buddies/{x.Image}"
                //Rating = x.Ra
            }).ToListAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = buddies, Message = "" };
            return responseModel;

        }

        public async Task<ResponseModel> InviteBuddy(int userBuddyId, Invitation_VM model)
        {
            ResponseModel responseModel = new ResponseModel();
            bool buddyExists = _uow.BuddyRepository.CheckIfExists(x => x.Id == model.ToBuddyId);
            bool placeExists = _uow.BuddyRepository.CheckIfExists(x => x.Id == model.PlaceId);
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
                 InvitationStatusId = 1
            };
            await _uow.InvitationRepository.Create(invitation);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 201;
            responseModel.Data = new DataModel { Data = "", Message = "Invitation Sent Succesfully" };
            return responseModel;
        }
    }
}
