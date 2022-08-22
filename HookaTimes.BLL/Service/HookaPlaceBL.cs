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

        public async Task<ResponseModel> GetHookaPlaces(HttpRequest request, int userBuddyId)
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
                IsInFavorite = p.FavoriteUserPlaces.Any(f => f.IsDeleted == false && f.BuddyId == userBuddyId)

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
                    Image = r.Buddy.Image,
                    Rating = (float)r.Rating
                }).ToList(),
                Name = p.Title,
                OpeningFrom = p.OpenningFrom,
                OpeningTo = p.OpenningTo,
                Latitude = p.Latitude,
                Longitude = p.Longitude
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

        public async Task<List<HookaPlaces_VM>> GetHookaPlacesMVC(HttpRequest request, int userBuddyId, int take = 0, List<int> cuisines = null, int sortBy = 0)
        {
            var query = _uow.PlaceRepository.GetAll(p => p.IsDeleted == false);
            List<HookaPlaces_VM> places = Array.Empty<HookaPlaces_VM>().ToList();

            if (cuisines != null)
            {
                if (cuisines.Count > 0)
                {
                    query = query.Where(p => cuisines.Contains((int)p.CuisineId));
                }
            }
            if (sortBy != default)
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
            if (take > 0)
            {
                query = query.Take(take);
            }
            places = await query.Select(p => new HookaPlaces_VM
            {
                Cuisine = p.Cuisine.Title,
                Id = p.Id,
                Image = p.Image,
                Name = p.Title,
                Location = p.Location.Title,
                Rating = (float)p.Rating,
                IsInFavorite = p.FavoriteUserPlaces.Where(f => f.IsDeleted == false && f.BuddyId == userBuddyId).Any()

            }).ToListAsync();
            return places;
        }

        public async Task<List<HookaPlaces_VM>> GetFavorites(int userBuddyId)
        {
            List<HookaPlaces_VM> favs = await _uow.FavoritePlaceRepository.GetAll(x => x.BuddyId == userBuddyId && x.IsDeleted == false).Select(p => new HookaPlaces_VM
            {
                Cuisine = p.PlaceProfile.Cuisine.Title,
                Id = p.PlaceProfile.Id,
                Image = p.PlaceProfile.Image,
                Name = p.PlaceProfile.Title,
                Location = p.PlaceProfile.Location.Title,
                Rating = (float)p.PlaceProfile.Rating,
                IsInFavorite = true
            }).ToListAsync();
            return favs;
        }

        public async Task<List<PlacesNames_VM>> GetPlacesNames()
        {
            List<PlacesNames_VM> places = await _uow.PlaceRepository.GetAll(x => x.IsDeleted == false).Select(p => new PlacesNames_VM
            {
                Id = p.Id,
                Title = p.Title
            }).ToListAsync();
            return places;
        }

        public async Task<ResponseModel> CreatePlace(CreateHookaPlace_vM model,string uid)
        {
            ResponseModel responseModel = new ResponseModel();
            PlacesProfile newPlace = new PlacesProfile()
            {
                CreatedDate = DateTime.UtcNow,
                CuisineId = model.CuisineId,
                Description = model.Description,
                IsDeleted = false,
                LocationId = model.LocationId,
                OpenningFrom = model.OpeningFrom,
                OpenningTo = model.OpeningTo,
                Title = model.Title,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                PhoneNumber = model.PhoneNumber,
                UserId = uid,
                 Rating = model.Rating,
            };
            var imgFile = model.Image;
            if(imgFile != null)
            {
                string path = await Helpers.SaveFile("wwwroot/images/places", imgFile);
                newPlace.Image = path;
            }
            newPlace =  await _uow.PlaceRepository.Create(newPlace);
            PlaceAlbum album = new PlaceAlbum();
            PlaceMenu menu = new PlaceMenu();
            if (model.Albums.Count > 0)
            {
                foreach (var item in model.Albums)
                {
                    string path = await Helpers.SaveFile("wwwroot/images/albums", imgFile);
                    album = new PlaceAlbum()
                    {
                        CreatedDate = DateTime.UtcNow,
                        Image = path,
                        IsDeleted = false,
                        PlaceProfileId = newPlace.Id,

                    };
                    await _uow.PlaceAlbumRepository.Add(album);
                }
                
            }
            if (model.Menus.Count > 0)
            {
                foreach (var item in model.Menus)
                {
                    string path = await Helpers.SaveFile("wwwroot/images/menus", imgFile);
                    menu = new PlaceMenu()
                    {
                        CreatedDate = DateTime.UtcNow,
                        Image = path,
                        IsDeleted = false,
                        PlaceProfileId = newPlace.Id,

                    };
                    await _uow.PlaceMenuRepository.Add(menu);
                }

            }
            await _uow.SaveAsync();
            responseModel.StatusCode = 200;
            responseModel.ErrorMessage = "";
            responseModel.Data = new DataModel { Data = "", Message = "Place created" };
            return responseModel;
        }
    }
}
