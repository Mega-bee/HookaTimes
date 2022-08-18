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
using Xunit.Abstractions;

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
            bool isProductInCart = await _uow.CartRepository.CheckIfExists(p => p.BuddyId == userBuddyId && p.ProductId == productId);
            if (isProductInCart)
            {
                var currCartItem = await _uow.CartRepository.GetFirst(x => x.ProductId == productId && x.BuddyId == userBuddyId);
                currCartItem.Quantity += quantity;
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
           var addedItem = await _uow.CartRepository.Create(newItem);
            CartItem_VM cartItem_VM = await _uow.CartRepository.GetAll(x => x.Id == addedItem.Id).Select(c => new CartItem_VM
            {
                ItemId = c.ProductId,
                ProductName = c.Product.Title,
                ProductPrice = c.Product.CustomerFinalPrice,
                Quantity = c.Quantity,
                TotalPrice = c.Quantity * c.Product.CustomerFinalPrice,
                ProductImage = c.Product.Image,
            }).FirstOrDefaultAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 201;
            responseModel.Data = new DataModel()
            {
                Data = cartItem_VM,
                Message = "Product added to cart"
            };
            return responseModel;

        }

        public async Task<ResponseModel> GetCartSummary(HttpRequest request, int userBuddyId)
        {
            ResponseModel responseModel = new ResponseModel();
            CartSummary_VM cartSummary = new CartSummary_VM()
            {

                Items = await _uow.CartRepository.GetAll(c => c.BuddyId == userBuddyId).Select(c => new CartItem_VM
                {
                    ItemId = c.ProductId,
                    ProductName = c.Product.Title,
                    ProductPrice = c.Product.CustomerFinalPrice,
                    Quantity = c.Quantity,
                    TotalPrice = c.Quantity * c.Product.CustomerFinalPrice,
                    ProductImage = $"{request.Scheme}://{request.Host}/{c.Product.Image}",
                }).ToListAsync(),
            };
            cartSummary.TotalPrice = cartSummary.Items.Sum(x => x.TotalPrice);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel()
            {
                Data = cartSummary,
                Message = ""
            };
            return responseModel;
        }
        public async Task<CartSummary_VM> GetCartSummaryMVC(int userBuddyId, string cartSessionId)
        {
            CartSummary_VM cartSummary = new CartSummary_VM()
            {
                Items = Array.Empty<CartItem_VM>().ToList(),
                TotalPrice = 0,
            };
            if (userBuddyId > 0)
            {
                cartSummary.Items = await _uow.CartRepository.GetAll(c => c.BuddyId == userBuddyId).Select(c => new CartItem_VM
                {
                    ItemId = c.ProductId,
                    CategoryName = c.Product.ProductCategory.Title,
                    ProductName = c.Product.Title,
                    ProductPrice = c.Product.CustomerFinalPrice,
                    Quantity = c.Quantity,
                    TotalPrice = c.Quantity * c.Product.CustomerFinalPrice,
                    ProductImage = c.Product.ProductCategory.Image,
                }).ToListAsync();
                cartSummary.TotalPrice = cartSummary.Items.Sum(x => x.TotalPrice);
                return cartSummary;
            }
            cartSummary.Items = await _uow.VirtualCartRepository.GetAll(c => c.SessionCartId == cartSessionId).Select(c => new CartItem_VM
            {
                ItemId = c.ProductId,
                CategoryName = c.Product.ProductCategory.Title,
                ProductName = c.Product.Title,
                ProductPrice = c.Product.CustomerFinalPrice,
                Quantity = c.Quantity,
                TotalPrice = c.Quantity * c.Product.CustomerFinalPrice,
                ProductImage = c.Product.ProductCategory.Image,
            }).ToListAsync();
            cartSummary.TotalPrice = cartSummary.Items.Sum(x => x.TotalPrice);
            return cartSummary;
        }

        public async Task<ResponseModel> RemoveItemFromCart(int productId, int userBuddyId, string cartSessionId)
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
                var currItem = await _uow.CartRepository.GetFirst(x => x.BuddyId == userBuddyId && x.ProductId == productId);
                await _uow.CartRepository.Delete(currItem.Id);
                responseModel.ErrorMessage = "";
                responseModel.StatusCode = 200;
                responseModel.Data = new DataModel()
                {
                    Data = "",
                    Message = "Item removed from cart"
                };
                return responseModel;
            }
            else
            {
                var currItem = await _uow.VirtualCartRepository.GetFirst(x => x.SessionCartId == cartSessionId && x.ProductId == productId);
                await _uow.VirtualCartRepository.Delete(currItem.Id);
                responseModel.ErrorMessage = "";
                responseModel.StatusCode = 200;
                responseModel.Data = new DataModel()
                {
                    Data = "",
                    Message = "Item removed from cart"
                };
                return responseModel;
            }
        }

        public async Task<ResponseModel> AddToCartCookies(string cartSessionId, int productId, int quantity)
        {
            ResponseModel responseModel = new ResponseModel();
            bool isInCart = await _uow.VirtualCartRepository.CheckIfExists(x => x.SessionCartId == cartSessionId && x.ProductId == productId);
            if (isInCart)
            {
                var currCartItem = await _uow.VirtualCartRepository.GetFirst(x => x.ProductId == productId && x.SessionCartId == cartSessionId);
                currCartItem.Quantity += quantity;
                await _uow.VirtualCartRepository.Update(currCartItem);
                responseModel.ErrorMessage = "";
                responseModel.StatusCode = 201;
                responseModel.Data = new DataModel()
                {
                    Data = "",
                    Message = "Product quantity updated"
                };
                return responseModel;
            }
            VirtualCart cartItem = new VirtualCart()
            {
                SessionCartId = cartSessionId,
                ProductId = productId,
                Quantity = quantity
            };
            await _uow.VirtualCartRepository.Create(cartItem);

            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 201;
            responseModel.Data = new DataModel()
            {
                Data = "",
                Message = "Product added to cart"
            };
            return responseModel;
        }

        public async Task<ResponseModel> UpdateCart(List<UpdateCartItem_VM> items, int userBuddyId, string cartSessionId)
        {
            ResponseModel responseModel = new ResponseModel();
            if(userBuddyId > 0)
            {
                if(items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        var currItem = await _uow.CartRepository.GetAllWithTracking(x=> x.ProductId == item.Id && x.BuddyId == userBuddyId).FirstOrDefaultAsync();
                        currItem.Quantity = item.Quantity;
                        
                    }
                    await _uow.SaveAsync();
                    responseModel.ErrorMessage = "";
                    responseModel.StatusCode = 201;
                    responseModel.Data = new DataModel()
                    {
                        Data = "",
                        Message = "Cart updated"
                    };
                    return responseModel;
                }
                responseModel.ErrorMessage = "Item list is empty";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel()
                {
                    Data = "",
                    Message = ""
                };
                return responseModel;
            } else if(!string.IsNullOrEmpty(cartSessionId))
            {
                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        var currItem = await _uow.VirtualCartRepository.GetAllWithTracking(x =>  x.SessionCartId == cartSessionId && x.ProductId == item.Id).FirstOrDefaultAsync();
                        currItem.Quantity = item.Quantity;

                    }
                    await _uow.SaveAsync();
                    responseModel.ErrorMessage = "";
                    responseModel.StatusCode = 201;
                    responseModel.Data = new DataModel()
                    {
                        Data = "",
                        Message = "Cart updated"
                    };
                    return responseModel;
                }
                responseModel.ErrorMessage = "Item list is empty";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel()
                {
                    Data = "",
                    Message = ""
                };
                return responseModel;
            } else
            {
                responseModel.ErrorMessage = "No cart is associated with current user";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel()
                {
                    Data = "",
                    Message = ""
                };
                return responseModel;
            }

        }
    }
}
