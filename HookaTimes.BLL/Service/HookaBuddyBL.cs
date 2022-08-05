using AutoMapper;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class HookaBuddyBL : BaseBO, IHookaBuddyBL
    {
        public HookaBuddyBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<ResponseModel> GetBuddies(HttpRequest request,string uid)
        {
            ResponseModel responseModel = new ResponseModel();
            int buddyId = await _uow.BuddyRepository.GetAll(b => b.UserId == uid).Select(b => b.Id).FirstOrDefaultAsync();

            List<HookaBuddy_VM> buddies = await _uow.BuddyRepository.GetAll(x => x.IsDeleted == false && x.Id != buddyId).Select(x => new HookaBuddy_VM
            {
                About = x.About,
                Id = x.Id,
                IsAvailable = x.IsAvailable,
                Name = x.FirstName + " " + x.LastName,
                 Image = $"{request.Scheme}://{request.Host}/Images/Buddies/{x.Image}"
                //Rating = x.Ra
            }).ToListAsync();
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = buddies, Message = "" };
            return responseModel;

        }
    }
}
