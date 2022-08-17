using AutoMapper;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.DAL;
using HookaTimes.DAL.HookaTimesModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class WishlistBL : BaseBO, IWishlistBL
    {
        public WishlistBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<ResponseModel> AddToWishlist(int productId, string wishlistSessionId, int userBuddyId)
        {
            ResponseModel responseModel = new ResponseModel();
            bool productExists = await _uow.ProductRepository.CheckIfExists(p => p.Id == productId);
            if (!productExists)
            {
                responseModel.ErrorMessage = "Product not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel()
                {
                    Data = "",
                    Message = ""
                };
                return responseModel;
            }
            if (userBuddyId > 0)
            {
                bool isProductInWishlist = await _uow.WishlistRepository.CheckIfExists(p => p.BuddyId == userBuddyId && p.ProductId == productId);
                if (isProductInWishlist)
                {
                    var currWishlistItem = await _uow.WishlistRepository.GetFirst(x => x.ProductId == productId && x.BuddyId == userBuddyId);

                    currWishlistItem.IsDeleted = true;
                    await _uow.WishlistRepository.Update(currWishlistItem);
                    responseModel.ErrorMessage = "";
                    responseModel.StatusCode = 201;
                    responseModel.Data = new DataModel()
                    {
                        Data = "",
                        Message = "Product removed from wishlist"
                    };
                    return responseModel;
                }
                Wishlist newWishlistItem = new Wishlist()
                {
                    BuddyId = userBuddyId,
                    IsDeleted = false,
                    CreatedDate = DateTime.UtcNow,
                    ProductId = productId
                };
                await _uow.WishlistRepository.Create(newWishlistItem);
                responseModel.ErrorMessage = "";
                responseModel.StatusCode = 201;
                responseModel.Data = new DataModel()
                {
                    Data = "",
                    Message = "Product added to wishlist"
                };
                return responseModel;
            }
            else
            {
                bool isProductInWishlist = await _uow.VirtualWishlistRepository.CheckIfExists(p => p.WishlistSessionId == wishlistSessionId && p.ProductId == productId);
                if (isProductInWishlist)
                {
                    var currWishlistItem = await _uow.VirtualWishlistRepository.GetFirst(x => x.ProductId == productId && x.WishlistSessionId == wishlistSessionId);

                    await _uow.VirtualWishlistRepository.Delete(currWishlistItem.Id);
                    responseModel.ErrorMessage = "";
                    responseModel.StatusCode = 204;
                    responseModel.Data = new DataModel()
                    {
                        Data = "",
                        Message = "Product removed from wishlist"
                    };
                    return responseModel;
                }
                VirtualWishlist newWishlistItem = new VirtualWishlist()
                {
                    ProductId = productId,
                    WishlistSessionId = wishlistSessionId,
                };
                await _uow.VirtualWishlistRepository.Create(newWishlistItem);
                responseModel.ErrorMessage = "";
                responseModel.StatusCode = 201;
                responseModel.Data = new DataModel()
                {
                    Data = "",
                    Message = "Product added to wishlist"
                };
                return responseModel;
            }

        }

        public async Task<List<Wishlist_VM>> GetWishlist(int userBuddyId, string wishlistSessionId)
        {
            List<Wishlist_VM> wishlist = Array.Empty<Wishlist_VM>().ToList();
            if (userBuddyId > 0)
            {
                wishlist = await _uow.WishlistRepository.GetAll(c => c.BuddyId == userBuddyId).Select(c => new Wishlist_VM
                {
                    ItemId = c.ProductId,
                    CategoryName = c.Product.ProductCategory.Title,
                    ProductName = c.Product.Title,
                    ProductPrice = c.Product.CustomerFinalPrice,
                    ProductImage = c.Product.ProductCategory.Image,
                }).ToListAsync();
               
                return wishlist;
            }
            wishlist = await _uow.VirtualWishlistRepository.GetAll(c => c.WishlistSessionId == wishlistSessionId).Select(c => new Wishlist_VM
            {
                ItemId = c.ProductId,
                CategoryName = c.Product.ProductCategory.Title,
                ProductName = c.Product.Title,
                ProductPrice = c.Product.CustomerFinalPrice,
                ProductImage = c.Product.ProductCategory.Image,
            }).ToListAsync();
       
            return wishlist;
        }

        public async Task<int> GetWishlistCount(int userBuddyId, string wishlistSessionId)
        {
            int count = 0;
            if (userBuddyId > 0)
            {
                count = await _uow.WishlistRepository.GetAll(c => c.BuddyId == userBuddyId).CountAsync();

                return count;
            }
            count = await _uow.VirtualWishlistRepository.GetAll(c => c.WishlistSessionId == wishlistSessionId).CountAsync();

            return count;
        }

        public async Task<ResponseModel> RemoveItemFromWishlist(int productId, int userBuddyId, string wishlistSessionId)
        {
            ResponseModel responseModel = new ResponseModel();
            bool productExists = await _uow.ProductRepository.CheckIfExists(p => p.Id == productId);
            if (!productExists)
            {
                responseModel.ErrorMessage = "Product not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel()
                {
                    Data = "",
                    Message = ""
                };
                return responseModel;
            }
            if (userBuddyId > 0)
            {
                var currItem = await _uow.WishlistRepository.GetFirst(x => x.BuddyId == userBuddyId && x.ProductId == productId);

                await _uow.WishlistRepository.Delete(currItem.Id);
                responseModel.ErrorMessage = "";
                responseModel.StatusCode = 200;
                responseModel.Data = new DataModel()
                {
                    Data = "",
                    Message = "Item removed from wishlist"
                };
                return responseModel;
            }
            else
            {
                var currItem = await _uow.VirtualWishlistRepository.GetFirst(x => x.WishlistSessionId == wishlistSessionId && x.ProductId == productId);
                await _uow.VirtualWishlistRepository.Delete(currItem.Id);
                responseModel.ErrorMessage = "";
                responseModel.StatusCode = 200;
                responseModel.Data = new DataModel()
                {
                    Data = "",
                    Message = "Item removed from wishlist"
                };
                return responseModel;
            }
        }
    }
}
