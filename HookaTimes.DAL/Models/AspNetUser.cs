using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AccProfiles = new HashSet<AccProfile>();
            Anomalies = new HashSet<Anomaly>();
            Devices = new HashSet<Device>();
            Notifications = new HashSet<Notification>();
            UserDevicetokens = new HashSet<UserDevicetoken>();
        }

        [Key]
        public string Id { get; set; }
        [StringLength(256)]
        public string UserName { get; set; }
        [StringLength(256)]
        public string NormalizedUserName { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(256)]
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string DeviceToken { get; set; }
        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }
        public int? GenderId { get; set; }

        [ForeignKey(nameof(GenderId))]
        [InverseProperty("AspNetUsers")]
        public virtual Gender Gender { get; set; }
        [InverseProperty(nameof(AccProfile.User))]
        public virtual ICollection<AccProfile> AccProfiles { get; set; }
        [InverseProperty(nameof(Anomaly.MonitoredByNavigation))]
        public virtual ICollection<Anomaly> Anomalies { get; set; }
        [InverseProperty(nameof(Device.CreatedByNavigation))]
        public virtual ICollection<Device> Devices { get; set; }
        [InverseProperty(nameof(Notification.User))]
        public virtual ICollection<Notification> Notifications { get; set; }
        [InverseProperty(nameof(UserDevicetoken.User))]
        public virtual ICollection<UserDevicetoken> UserDevicetokens { get; set; }
    }
}
