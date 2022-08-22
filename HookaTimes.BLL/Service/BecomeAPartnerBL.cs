using AutoMapper;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using HookaTimes.DAL;
using HookaTimes.DAL.HookaTimesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class BecomeAPartnerBL : BaseBO, IBecomeAPartnerBL
    {
        public BecomeAPartnerBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<ResponseModel> SendPartnerRequest(BecomeAPartner_VM model)
        {
            ResponseModel responseModel = new ResponseModel();
            PartnerRequest newReq = new PartnerRequest()
            {
                Description = model.Description,
                EmailAddress = model.EmailAddress,
                FirstName = model.FirstName,
                LastName = model.LastName,
                HookaPlaceName = model.HookaPlaceName,
                PhoneNumber = model.PhoneNumber,
                 Location = model.Location,
            };
            await _uow.PartnerRequestRepository.Create(newReq);
            responseModel.StatusCode = 200;
            responseModel.ErrorMessage = "";
            responseModel.Data = new DataModel { Data = "", Message = "Your submission has been sent" };
            return responseModel;
        }
    }
}
