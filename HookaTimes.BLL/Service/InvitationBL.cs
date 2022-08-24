using AutoMapper;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.DAL;
using HookaTimes.DAL.HookaTimesModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class InvitationBL : BaseBO, IInvitationBL
    {
        public InvitationBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        #region Api
        public async Task<ResponseModel> SetInvitationStatus(int statusId, int invitationId)
        {
            ResponseModel responseModel = new ResponseModel();
            Invitation invitation = await _uow.InvitationRepository.GetFirst(x => x.Id == invitationId);
            if (invitation == null)
            {
                responseModel.ErrorMessage = "Invitaiton not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            invitation.InvitationStatusId = statusId;
            Invitation test = await _uow.InvitationRepository.Update(invitation);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 201;
            responseModel.Data = new DataModel { Data = "", Message = "Invitation status sucessfully set" };
            return responseModel;

        }

        public async Task<ResponseModel> GetSentInvitations(HttpRequest request, int userBuddyId)
        {
            ResponseModel responseModel = new ResponseModel();
            var invitationList = await _uow.InvitationRepository.GetAllWithPredicateAndIncludes(x => x.FromBuddyId == userBuddyId, x => x.Place).ToListAsync();
            List<SentInvitation_VM> invitations = invitationList.GroupBy(x => x.PlaceId).Select(x => new SentInvitation_VM
            {
                PlaceId = (int)x.Select(i => i.PlaceId).FirstOrDefault(),
                BuddiesCount = x.Count(),
                PlaceName = x.Select(i => i.Place.Title).FirstOrDefault(),
                Image = $"{request.Scheme}://{request.Host}{x.Select(i => i.Place.Image).FirstOrDefault()}",
                Rating = (float)x.Select(i => i.Place.Rating).FirstOrDefault()

            }).ToList();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = invitations, Message = "" };
            return responseModel;
        }
        public async Task<ResponseModel> GetRecievedInvitations(HttpRequest request, int userBuddyId)
        {
            ResponseModel responseModel = new ResponseModel();
            List<Invitation_VM> invitations = await _uow.InvitationRepository.GetAll(x => x.ToBuddyId == userBuddyId).Select(i => new Invitation_VM
            {
                Description = i.Description ?? "",
                BuddyName = i.FromBuddy.FirstName + " " + i.FromBuddy.LastName,
                InvitationStatusId = (int)i.InvitationStatusId,
                BuddyRating = 0,
                InvitationStatus = i.InvitationStatus.Title,
                Id = i.Id,
                BuddyImage = $"{request.Scheme}://{request.Host}{i.FromBuddy.Image}",
                RestaurantName = i.Place.Title,
                InvitationOption = i.InvitationOption.Title,
                InvitationDate = i.InvitationDate,

            }).OrderBy(x => x.InvitationStatusId).ToListAsync();

            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = invitations, Message = "" };
            return responseModel;
        }

        public async Task<ResponseModel> GetInvitationOptions()
        {
            ResponseModel responseModel = new ResponseModel();
            List<InvitationOption_VM> options = await _uow.InvitationOptionRepository.GetAll(x => x.IsDeleted == false).Select(x => new InvitationOption_VM
            {
                Id = x.Id,
                Title = x.Title,
            }).ToListAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = options, Message = "" };
            return responseModel;
        }

        public async Task<ResponseModel> GetPlaceInvitations(HttpRequest request, int placeId, int userBuddyId)
        {
            ResponseModel responseModel = new ResponseModel();
            PlaceInvitation_VM invitations = await _uow.PlaceRepository.GetAll(p => p.Id == placeId).Select(p => new PlaceInvitation_VM
            {
                PlaceId = p.Id,
                PlaceLocation = p.Location.Title,
                PlaceImage = $"{request.Scheme}://{request.Host}{p.Image}",
                PlaceName = p.Title,
                PlaceRating = (float)p.Rating,
                Buddies = p.Invitations.Where(p => p.FromBuddyId == userBuddyId).Select(i => new Invitation_VM
                {
                    Description = i.Description ?? "",
                    BuddyName = i.ToBuddy.FirstName + " " + i.ToBuddy.LastName,
                    InvitationStatusId = (int)i.InvitationStatusId,
                    BuddyRating = (float?)i.ToBuddy.Rating,
                    InvitationStatus = i.InvitationStatus.Title,
                    Id = i.Id,
                    BuddyImage = $"{request.Scheme}://{request.Host}{i.ToBuddy.Image}",
                }).ToList(),

            }).FirstOrDefaultAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = invitations, Message = "" };
            return responseModel;
        }
        #endregion



        #region MVC


        #endregion

    }
}
