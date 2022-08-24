using AutoMapper;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class CuisineBL : BaseBO, ICuisineBL
    {
        public CuisineBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper, INotificationBL notificationBL) : base(unit, mapper, notificationHelper, notificationBL)
        {
        }

        public async Task<List<Cuisine_VM>> GetCuisinesMVC()
        {
            List<Cuisine_VM> cuisines = await _uow.CuisineRepository.GetAll(c => c.IsDeleted == false).Select(c => new Cuisine_VM
            {
                Id = c.Id,
                Title = c.Title
            }).ToListAsync();
            return cuisines;
        }

        public async Task<ResponseModel> GetCuisines()
        {
            ResponseModel responseModel = new ResponseModel();
            List<Cuisine_VM> data = await GetCuisinesMVC();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = data, Message = "" };
            return responseModel;
        }
    }
}
