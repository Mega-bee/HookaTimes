﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels
{
    public partial class NotificationModel
    {
        public string DeviceId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }


    }
}
