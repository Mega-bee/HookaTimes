using System;

namespace HookaTimes.BLL.ViewModels
{
    public partial class NotificationModel
    {
        public string DeviceId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int? BuddyId { get; set; }
        public int? OrderId { get; set; }
        public int InviteId { get; set; }


    }

    public partial class Notification_VM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int OrderId { get; set; }
        public int InviteId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
