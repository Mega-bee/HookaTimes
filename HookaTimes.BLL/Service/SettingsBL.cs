using AutoMapper;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class SettingsBL : BaseBO, ISettingsBL
    {
        public SettingsBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<ResponseModel> GetSettings(int userBuddyId)
        {
            ResponseModel responseModel = new ResponseModel();
            bool isAvailable = (bool)await _uow.BuddyRepository.GetAll(x => x.Id == userBuddyId).Select(x => x.IsAvailable).FirstOrDefaultAsync();
            AccountSettings_VM settings = new AccountSettings_VM()
            {
                IsAvailable = isAvailable,
                SocialMediaLinks = await _uow.ContactInfoRepository.GetAll().Select(x => new SocialMediaLinks_VM
                {
                    SocialMediaLink1 = x.SocialMediaLink1 ?? "",
                    SocialMediaLink2 = x.SocialMediaLink2 ?? "",
                    SocialMediaLink3 = x.SocialMediaLink3 ?? ""
                }).FirstOrDefaultAsync()
            };
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = settings, Message = "" };
            return responseModel;
        }
    }
}
