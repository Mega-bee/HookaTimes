using AutoMapper;
using HookaTimes.BLL.Enums;
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
    public class OrderBL : BaseBO, IOrderBL
    {
        public OrderBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<ResponseModel> PlaceOrder(int userBuddyId, int addressId)
        {
            ResponseModel responseModel = new ResponseModel();
            OrderItem orderItem = new OrderItem();
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
            }

            order.Total = orderItems.Sum(x => x.Quantity * x.Price);
            await _uow.OrderRepository.Update(order);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = "", Message = "Order Placed Successfully" };
            return responseModel;
        }


    }
}

