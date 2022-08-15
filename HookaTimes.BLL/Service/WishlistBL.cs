using AutoMapper;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.DAL;
using HookaTimes.DAL.HookaTimesModels;
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
                    responseModel.StatusCode = 201;
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
    }
}
