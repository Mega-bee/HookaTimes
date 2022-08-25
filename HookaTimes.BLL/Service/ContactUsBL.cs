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
    public class ContactUsBL : BaseBO, IContactUsBL
    {
        public ContactUsBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper, INotificationBL notificationBL) : base(unit, mapper, notificationHelper, notificationBL)
        {
        }

        public async Task<ResponseModel> SendContactUsMessage(ContactUs_VM model)
        {
            ResponseModel responseModel = new ResponseModel();
            ContactU newContactUs = new ContactU()
            {
                Email = model.Email,
                Message = model.Message,
                Name = model.Email,
                PhoneNumber = model.Mobile
            };
            await _uow.ContactUsRepository.Create(newContactUs);
            responseModel.StatusCode = 200;
            responseModel.ErrorMessage = "";
            responseModel.Data = new DataModel { Data = "", Message = "Your message has been sent" };
            return responseModel;
        }
    }
}
