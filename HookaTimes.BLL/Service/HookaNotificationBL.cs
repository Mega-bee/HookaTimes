using AutoMapper;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class HookaNotificationBL : BaseBO, IHookaNotificationBL
    {
        public HookaNotificationBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<ResponseModel> GetSentNotification(HttpRequest request, int buddyId)
        {
            var responseModel = new ResponseModel();

            List<SentNotifications_VM> sentNot = _uow.SentNotificationsRepository.GetAll(x => x.IsDeleted == false).ToList().GroupBy(i => i.PlaceId).Select(x => new SentNotifications_VM
            {
                Id=x.FirstOrDefault().PlaceId,
                Title=x.FirstOrDefault().Place.Title,
                Image = $"{request.Scheme}://{request.Host}/Images/Places/{x.FirstOrDefault().Place.Image}",
                NumbBuddies=x.Count(),

            }).ToList();

            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = sentNot, Message = "" };
            return responseModel;

        }

        public async Task<ResponseModel> GetInvDetail(int placeId)
        {
            var responseModel = new ResponseModel();

            InvDetail_VM invDetail = _uow.InvDetailRepository.GetAll(x=>x.Id==placeId).Select(x=> new InvDetail_VM
            {
                Id = x.Id,
                Image=x.Image,
                Title=x.Title,
                LocationTitle=x.Location.Title,
                Rating=x.Rating,
                Buddies=new List<buddies_VM>(),
            }).FirstOrDefault();

            var buddies = _uow.BuddyRepository.GetAll().Select(x => new buddies_VM
            {
                Fname = x.FirstName,
                Lname = x.LastName,

            }).ToList();

            responseModel.ErrorMessage = "";
            responseModel.StatusCode=200;
            responseModel.Data = new DataModel { Data = invDetail, Message = "" };
            return responseModel;
        }

    }
}
