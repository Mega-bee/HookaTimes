using AutoMapper;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.DAL;
using HookaTimes.DAL.HookaTimesModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class NotificationBL : INotificationBL
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _fcmSettings;
        private readonly IUnitOfWork _uow;
        public NotificationBL(IUnitOfWork unit,IConfiguration configuration) 
        {
             _uow = unit;
            _configuration = configuration;
            _fcmSettings = _configuration.GetSection("FcmNotification");
        }

        public async Task<bool> SendNotification(NotificationModel notificationModel)
        {

            Notification notification = new Notification()
            {
                BuddyId = notificationModel.BuddyId,
                CreatedDate = DateTime.UtcNow,
                Description = notificationModel.Body,
                IsSeen = false,
                OrderId = notificationModel.OrderId,
            };
            await _uow.NotificationRepository.Create(notification);
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";
            var objNotification = new
            {
                to = notificationModel.DeviceId,
                notification = new
                {
                    title = notificationModel.Title,
                    body = notificationModel.Body,
                    orderId = notificationModel.OrderId,
                    inviteId = notificationModel.InviteId,

                    sound = "default",
                    //android_channel_id= "easyapproach"
                },
                android = new
                {
                    priority = "HIGH",
                    notification = new
                    {
                        notification_priority = "PRIORITY_MAX",
                        sound = "default",
                        default_sound = true,
                        default_vibrate_timing = true,
                        default_light_settings = true
                    }
                },
                data = new
                {
                    type = "order",
                    title = notificationModel.Title,
                    body = notificationModel.Body,
                    orderId = notificationModel.OrderId,
                    inviteId = notificationModel.InviteId,
                    //routePage= "order",
                    click_action = "FLUTTER_NOTIFICATION_CLICK"
                }
            };

            string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objNotification);

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
            tRequest.Headers.Add(string.Format("Authorization: key={0}", _fcmSettings.GetSection("serverKey").Value));
            tRequest.Headers.Add(string.Format("Sender: id={0}", _fcmSettings.GetSection("senderId").Value));
            tRequest.ContentLength = byteArray.Length;
            tRequest.ContentType = "application/json";
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            string responseFromFirebaseServer = tReader.ReadToEnd();

                            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(responseFromFirebaseServer);
                            if (response != null)
                            {
                                //var prof = _context.Orders.Where(x => x.Id == notificationModel.OrderId)
                                //           .Select(x => new
                                //           {
                                //               x.ProfileId
                                //           }).FirstOrDefault();

                                return true;
                            }
                            else
                            {
                                return false;

                            }

                        }
                    }

                }
            }





        }
    }
}
