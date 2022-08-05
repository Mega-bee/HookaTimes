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
    public class InvitationBL : BaseBO, IInvitationBL
    {
        public InvitationBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }

        public async Task<ResponseModel> SetInvitationStatus(int statusId, int invitationId)
        {
            ResponseModel responseModel = new ResponseModel();
            Invitation invitation = await _uow.InvitationRepository.GetFirst(x => x.Id == statusId);
            if (invitation == null)
            {
                responseModel.ErrorMessage = "Invitaiton not found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;
            }
            invitation.InvitationStatusId = statusId;
            await _uow.InvitationRepository.Update(invitation);
            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 201;
            responseModel.Data = new DataModel { Data = "", Message = "Invitation status sucessfully set" };
            return responseModel;

        }
    }
}
