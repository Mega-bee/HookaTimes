using AutoMapper;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class OfferBL : BaseBO, IOfferBL
    {
        public OfferBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper, INotificationBL notificationBL) : base(unit, mapper, notificationHelper, notificationBL)
        {
        }


        #region OfferList
        public async Task<ResponseModel> GetOfferList(HttpRequest request)
        {
            ResponseModel responseModel = new ResponseModel();
            List<OfferList_VM> offers = await _uow.PlaceOfferRepository.GetAllWithPredicateAndIncludes(x => x.IsDeleted == false, x => x.PlaceProfile).Select(o => new OfferList_VM
            {
                Id = o.Id,
                Image = $"{request.Scheme}://{request.Host}/{o.Image}",
                Rating = (float)o.PlaceProfile.Rating,
                RestaurantTitle = o.PlaceProfile.Title,
                Title = o.Title,
            }).ToListAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = offers, Message = "" };
            return responseModel;

        }
        #endregion

        #region OfferByID
        public async Task<ResponseModel> GetOfferById(int id, HttpRequest request)
        {
            ResponseModel responseModel = new ResponseModel();
            bool OfferExist = await _uow.PlaceOfferRepository.CheckIfExists(x => x.Id == id && x.IsDeleted == false);
            if (!OfferExist)
            {
                responseModel.ErrorMessage = "Offer was not Found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            Offer_VM offer = await _uow.PlaceOfferRepository.GetAll(x => x.Id == id && x.IsDeleted == false).Select(o => new Offer_VM
            {
                Type = o.Type.Title,
                Cuisine = o.PlaceProfile.Cuisine.Title,
                Discount = o.Discount.ToString(),
                ExpiryDate = o.ExpiryDate.Value,
                Image = $"{request.Scheme}://{request.Host}/{o.Image}",
                Latitude = o.PlaceProfile.Latitude,
                Longitude = o.PlaceProfile.Longitude,
                Location = o.PlaceProfile.Location.Title,
                OfferDescription = o.Description,
                OfferTitle = o.Title,
                OpenningFrom = o.PlaceProfile.OpenningFrom,
                OpenningTo = o.PlaceProfile.OpenningTo,
                PhonNumber = o.PlaceProfile.PhoneNumber,
                Rating = o.PlaceProfile.Rating.ToString(),
                RestaurantDescription = o.PlaceProfile.Description,
                RestaurantName = o.PlaceProfile.Title
            }).FirstOrDefaultAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = offer, Message = "" };
            return responseModel;

        }
        #endregion

    }
}
