using AutoMapper;
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
                Image = $"{request.Scheme}://{request.Host}{p.Image}",
                Name = p.Title,
                Location = p.Location.Title,
                Rating = (float)p.Rating,
                 
            }).ToListAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = places, Message = "" };
            return responseModel;
        }

        public async Task<ResponseModel> GetHookaPlace(HttpRequest request, int userBuddyId, int id)
        {
            ResponseModel responseModel = new ResponseModel();

            HookaPlaceInfo_VM place = await _uow.PlaceRepository.GetAll(p => p.IsDeleted == false && p.Id == id).Select(p => new HookaPlaceInfo_VM
            {
                Cuisine = p.Cuisine.Title,
                Id = p.Id,
                Image = $"{request.Scheme}://{request.Host}{p.Image}",
                Location = p.Location.Title,
                Rating = (float)p.Rating,
                Albums = p.PlaceAlbums.Where(a => a.IsDeleted == false).Select(a => new HookaPlaceImage_VM
                {
                    Id = a.Id,
                    Image = $"{request.Scheme}://{request.Host}{a.Image}",
                }).ToList(),
                Description = p.Description,
                IsFavorite = p.FavoriteUserPlaces.Any(f => f.BuddyId == userBuddyId && f.IsDeleted == false),
                Favorites = p.FavoriteUserPlaces.Where(f => f.IsDeleted == false).Select(f => new HookaPlaceFavorite_VM
                {
                    Id = f.Id,
                    Image = $"{request.Scheme}://{request.Host}{f.Buddy.Image}",
                    IsAvailable = f.Buddy.IsAvailable
                }).ToList(),
                Menus = p.PlaceMenus.Where(m => m.IsDeleted == false).Select(m => new HookaPlaceImage_VM
                {
                    Id = m.Id,
                    Image = $"{request.Scheme}://{request.Host}{m.Image}"
                }).ToList(),
                Reviews = p.PlaceReviews.Where(r => r.IsDeleted == false).Select(r => new HookaPlaceReview_VM
                {
                    CreatedDate = r.CreatedDate,
                    Description = r.Description,
                    Id = r.Id,
                    Name = r.Buddy.FirstName + " " + r.Buddy.LastName,
                    Rating = (float)r.Rating
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
            bool placeExists = await _uow.BuddyRepository.CheckIfExists(x => x.Id == placeId);
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

        //public async Task<ResponseModel> AddReview(HookaPlaceReview_VM model, HttpRequest request, int id, int BuddyId)
        //{

        //    PlaceReview placeReview = _context.PlaceReviews.Where(x => x.PlaceProfileId == id).FirstOrDefault();
        //    ResponseModel responseModel = new ResponseModel();

        //    if (placeReview is null)
        //    {
        //        responseModel.StatusCode = 404;
        //        responseModel.ErrorMessage = "Place was not Found";
        //        responseModel.Data = new DataModel { Data = "", Message = "" };
        //        return responseModel;
        //    }

        //    placeReview.IsDeleted = false;
        //    placeReview.CreatedDate = DateTime.UtcNow;
        //    placeReview.BuddyId = BuddyId;
        //    placeReview.Rating = model.Rating;
        //    placeReview.Description = model.Description;

        //    await _context.SaveChangesAsync();

        //    HookaPlaceInfo_VM hookaProfile = new HookaPlaceInfo_VM
        //    {
        //        Id = placeReview.Id,
        //    };

        //    responseModel.StatusCode = 200;
        //    responseModel.ErrorMessage = "";
        //    responseModel.Data = new DataModel
        //    {
        //        Data = hookaProfile,
        //        Message = ""
        //    };
        //    return responseModel;

        //}

        public async Task<ResponseModel> AddReview(CreateReview_VM model, HttpRequest request, int id, int buddyId)
        {
            //PlacesProfile placesProfile = _context.PlacesProfiles.Where(x => x.Id == id).FirstOrDefault();

            bool placesProfile = await _uow.PlaceRepository.CheckIfExists(x => x.Id == id);
            ResponseModel responseModel = new ResponseModel();

            if (placesProfile is false)
            {
                responseModel.StatusCode = 404;
                responseModel.ErrorMessage = "Place was not Found";
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }

            PlaceReview placeReview = new PlaceReview
            {
                IsDeleted = false,
                CreatedDate = DateTime.Now,
                BuddyId = buddyId,
                PlaceProfileId = id,
                Rating = model.Rating,
                Description = model.Description
            };

            await _uow.PlaceReviewRepository.Create(placeReview);

            //HookaPlaceInfo_VM hookaProfile = new HookaPlaceInfo_VM
            //{
            //    Id = placeReview.Id,
            //};

            responseModel.StatusCode = 200;
            responseModel.ErrorMessage = "";
            responseModel.Data = new DataModel
            {
                Data = "",
                Message = "Review added successfuly"
            };
            return responseModel;

        }

        public async Task<List<HookaPlaces_VM>> GetHookaPlacesMVC(HttpRequest request,int take=0,List<int> cuisines = null,int sortBy = 0)
        {
            var query = _uow.PlaceRepository.GetAll(p => p.IsDeleted == false);
            List<HookaPlaces_VM> places = Array.Empty<HookaPlaces_VM>().ToList();
    
            if(cuisines != null)
            {
                if(cuisines.Count > 0)
                {
                    query = query.Where(p => cuisines.Contains((int)p.CuisineId));
                }
            }
            if(sortBy != default)
            {
                switch (sortBy)
                {
                    case 1:
                        query = query.OrderByDescending(p => p.Rating);
                        break;
                    default:
                        break;
                }
            }
            if(take>0)
            {
                query = query.Take(take);
            }
             places = await query.Select(p => new HookaPlaces_VM
            {
                Cuisine = p.Cuisine.Title,
                Id = p.Id,
                Image = $"{request.Scheme}://{request.Host}{p.Image}",
                Name = p.Title,
                Location = p.Location.Title,
                Rating = (float)p.Rating,

            }).ToListAsync();
            return places;
        }

    }
}
