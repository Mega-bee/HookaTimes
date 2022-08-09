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
    public class CartBL : BaseBO, ICartBL
    {
        public CartBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<ResponseModel> AddToCart(int userBuddyId, int quantity, int productId)
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
            bool isProductInCart = await _uow.CartRepository.CheckIfExists(p => p.BuddyId == userBuddyId && p.Id == productId);
            if (isProductInCart)
            {
                var currCartItem = await _uow.CartRepository.GetFirst(x => x.ProductId == productId && x.BuddyId == userBuddyId);
                currCartItem.Quantity = quantity;
                await _uow.CartRepository.Update(currCartItem);
                responseModel.ErrorMessage = "";
                responseModel.StatusCode = 201;
                responseModel.Data = new DataModel()
                {
                    Data = "",
                    Message = "Product quantity updated"
                };
                return responseModel;
            }
            Cart newItem = new Cart()
            {
                BuddyId = userBuddyId,
                CreatedDate = DateTime.UtcNow,
                ProductId = productId,
                Quantity = quantity,
            };
            await _uow.CartRepository.Create(newItem);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 201;
            responseModel.Data = new DataModel()
            {
                Data = "",
                Message = "Product added to cart"
            };
            return responseModel;

        }
    }
}
