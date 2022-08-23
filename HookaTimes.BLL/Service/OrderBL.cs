using AutoMapper;
using HookaTimes.BLL.Enums;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using HookaTimes.DAL;
using HookaTimes.DAL.HookaTimesModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class OrderBL : BaseBO, IOrderBL
    {
        public OrderBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<ResponseModel> PlaceOrder(int userBuddyId, int addressId, BuddyProfileAddressVM address = null)
        {
            ResponseModel responseModel = new ResponseModel();
            OrderItem orderItem = new OrderItem();
            if (addressId == 0)
            {
                BuddyProfileAddress newAddress = new BuddyProfileAddress()
                {
                    Apartment = address.Building,
                    Building = address.Building,
                    City = address.City,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false,
                    BuddyProfileId = userBuddyId,
                    Title = address.Title,
                    Street = address.Street,

                };
                newAddress = await _uow.BuddyProfileAddressRepository.Create(newAddress);
                addressId = newAddress.Id;
            }
            Order order = new Order()
            {
                AddressId = addressId,
                BuddyId = userBuddyId,
                CreatedDate = DateTime.UtcNow,
                OrderStatusId = (int)OrderStatuses.Pending
            };
            order = await _uow.OrderRepository.Create(order);
            List<OrderItem> orderItems = await _uow.CartRepository.GetAll(x => x.BuddyId == userBuddyId).Select(x => new OrderItem
            {
                CreatedAt = DateTime.UtcNow,
                OrderId = order.Id,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Price = x.Product.CustomerFinalPrice,
            }).ToListAsync();
            if (orderItems.Count > 0)
            {
                await _uow.OrderItemRepository.AddRange(orderItems);
                await _uow.SaveAsync();
                var cartItems = await _uow.CartRepository.GetAll(x => x.BuddyId == userBuddyId).ToListAsync();
                await _uow.CartRepository.DeleteRange(cartItems);
            }

            order.Total = orderItems.Sum(x => x.Quantity * x.Price);
            await _uow.OrderRepository.Update(order);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = "", Message = "Order Placed Successfully" };
            return responseModel;
        }

        public async Task<ResponseModel> GetOrders(int userBuddyId)
        {
            ResponseModel responseModel = new ResponseModel();
            OrderHistory_VM history = new OrderHistory_VM()
            {
                CurrentOrders = await _uow.OrderRepository.GetAll(x => x.BuddyId == userBuddyId && x.OrderStatusId == 1).Select(x => new OrderHistoryMVC_VM
                {
                    Id = x.Id,
                    Date = x.CreatedDate.Value.ToString("dd MMMM, yyyy"),
                    Status = x.OrderStatus.Title,
                    Total = Convert.ToDecimal(x.Total.Value.ToString("0.##")),

                }).ToListAsync(),
                AllOrders = await _uow.OrderRepository.GetAll(x => x.BuddyId == userBuddyId).Select(x => new OrderHistoryMVC_VM
                {
                    Id = x.Id,
                    Date = x.CreatedDate.Value.ToString("dd MMMM, yyyy"),
                    Status = x.OrderStatus.Title,
                    Total = Convert.ToDecimal(x.Total.Value.ToString("0.##")),

                }).ToListAsync()
        };
          
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = history, Message = "" };
            return responseModel;
        }

        public async Task<ResponseModel> GetOrder(HttpRequest request,int userBuddyId,int orderId)
        {
            ResponseModel responseModel = new ResponseModel();

            OrderDetails_VM order = await _uow.OrderRepository.GetAll(x => x.BuddyId == userBuddyId && x.Id == orderId).Select(x => new OrderDetails_VM
            {
                Id = x.Id,
                Date = x.CreatedDate.Value.ToString("dd MMMM, yyyy"),
                Status = x.OrderStatus.Title,
                Total = Convert.ToDecimal(x.Total.Value.ToString("0.##")),
                Address = new BuddyProfileAddressVM
                {
                    Appartment = x.Address.Apartment,
                    Building = x.Address.Building,
                    City = x.Address.City,
                    Id = x.Id,
                    Street = x.Address.Street,
                    Title = x.Address.Title,
                    Latitude = x.Address.Latitude,
                    Longitude = x.Address.Longitude
                },
                 Items = x.OrderItems.Select(i=> new CartItem_VM
                 {
                      CategoryId = (int)i.Product.ProductCategoryId,
                        CategoryName = i.Product.ProductCategory.Title,
                     ItemId = i.ProductId,
                     ProductImage = $"{request.Scheme}://{request.Host}{i.Product.Image}",
                      ProductName = i.Product.Title,
                       ProductPrice = Convert.ToDecimal(i.Product.CustomerFinalPrice.Value.ToString("0.##")),
                        Quantity =i.Quantity,
                         TotalPrice = Convert.ToDecimal((i.Quantity* i.Product.CustomerFinalPrice).Value.ToString("0.##")),
                          
                 }).ToList(),
                  

            }).FirstOrDefaultAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = order, Message = "" };
            return responseModel;
        }

        


    }
}

