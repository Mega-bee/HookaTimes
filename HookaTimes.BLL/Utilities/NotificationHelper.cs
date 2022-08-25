using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
//using HookaTimes.DAL.Models;
using HookaTimes.BLL.ViewModels;
using HookaTimes.DAL.Data;

namespace HookaTimes.BLL.Utilities
{
    public class NotificationHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _fcmSettings;

        public NotificationHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _fcmSettings = _configuration.GetSection("FcmNotification");
        }
        public bool SendNotification(NotificationModel notificationModel)
        {
            try
            {

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
            catch (Exception ex)
            {
                return false;
                throw;
            }




        }


    }
}
