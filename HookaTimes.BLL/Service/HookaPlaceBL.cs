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
    public class HookaPlaceBL : BaseBO, IHookaPlaceBL
    {
        public HookaPlaceBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<ResponseModel> GetHookaPlaces(HttpRequest request)
        {
            ResponseModel responseModel = new ResponseModel();

            List<HookaPlaces_VM> places = await _uow.PlaceRepository.GetAll(p => p.IsDeleted == false).Select(p => new HookaPlaces_VM
            {
                Cuisine = p.Cuisine.Title,
                Id = p.Id,
                Image = $"{request.Scheme}://{request.Host}/Images/Places/{p.Image}",
                Name = p.Title,
                Location = p.Location.Title,
                Rating = (float)p.Rating
            }).ToListAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = places, Message = "" };
            return responseModel;
        }

        public async Task<ResponseModel> GetHookaPlace(HttpRequest request, int id)
        {
            ResponseModel responseModel = new ResponseModel();

            HookaPlaceInfo_VM place = await _uow.PlaceRepository.GetAll(p => p.IsDeleted == false && p.Id == id).Select(p => new HookaPlaceInfo_VM
            {
                Cuisine = p.Cuisine.Title,
                Id = p.Id,
                Image = $"{request.Scheme}://{request.Host}/Images/Places/{p.Image}",
                Location = p.Location.Title,
                Rating = (float)p.Rating,
                Albums = p.PlaceAlbums.Where(a => a.IsDeleted == false).Select(a => new HookaPlaceImage_VM
                {
                    Id = a.Id,
                    Image = $"{request.Scheme}://{request.Host}/Images/Albums/{a.Image}",
                }).ToList(),
                Description = p.Description,
                Favorites = p.FavoriteUserPlaces.Where(f => f.IsDeleted == false).Select(f => new HookaPlaceFavorite_VM
                {
                    Id = f.Id,
                    Image = $"{request.Scheme}://{request.Host}/Images/Buddies/{f.Buddy.Image}",
                    IsAvailable = f.Buddy.IsAvailable
                }).ToList(),
                Menus = p.PlaceMenus.Where(m => m.IsDeleted == false).Select(m => new HookaPlaceImage_VM
                {
                    Id = m.Id,
                    Image = $"{request.Scheme}://{request.Host}/Images/Menus/{m.Image}"
                }).ToList(),
                Reviews = p.PlaceReviews.Where(r => r.IsDeleted == false).Select(r => new HookaPlaceReview_VM
                {
                    CreatedDate = r.CreatedDate,
                    Description = r.Description,
                    Id = r.Id,
                    Name = r.Buddy.FirstName + " " + r.Buddy.LastName,

                }).ToList(),
                Name = p.Title,
                OpeningFrom = p.OpenningFrom,
                OpeningTo = p.OpenningTo,
            }).FirstOrDefaultAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = place, Message = "" };
            return responseModel;
        }

        public async Task<ResponseModel> AddToFavorites(string uid, int placeId)
        {
            ResponseModel responseModel = new ResponseModel();
            bool placeExists = _uow.BuddyRepository.CheckIfExists(x => x.Id == placeId);
            int buddyId = await _uow.BuddyRepository.GetAll(b => b.UserId == uid).Select(b => b.Id).FirstOrDefaultAsync();
            FavoriteUserPlace favorite = null;
            if (!placeExists)
            {
                responseModel.ErrorMessage = "Place not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            favorite = await _uow.FavoritePlaceRepository.GetFirst(x => x.BuddyId == buddyId && x.IsDeleted == false && x.PlaceProfileId == placeId);
            if (favorite != null)
            {
                favorite.IsDeleted = true;
                await _uow.FavoritePlaceRepository.Update(favorite);
                responseModel.ErrorMessage = "";
                responseModel.StatusCode = 200;
                responseModel.Data = new DataModel { Data = "", Message = "Place removed from favorites" };
                return responseModel;
            }
            else
            {
                favorite = new FavoriteUserPlace()
                {
                    BuddyId = buddyId,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false,
                    PlaceProfileId = placeId,
                     
                };
                await _uow.FavoritePlaceRepository.Create(favorite);
                responseModel.ErrorMessage = "";
                responseModel.StatusCode = 201;
                responseModel.Data = new DataModel { Data = "", Message = "Place added to favorites" };
                return responseModel;
            }
        }

    }
}
