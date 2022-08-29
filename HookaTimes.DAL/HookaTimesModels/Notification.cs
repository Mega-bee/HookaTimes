using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("Notification")]
    public partial class Notification
    {
        [Key]
        public int Id { get; set; }
        public int? BuddyId { get; set; }
        public int? OrderId { get; set; }
        public int? InviteId { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public bool? IsSeen { get; set; }

        [ForeignKey("BuddyId")]
        [InverseProperty("Notifications")]
        public virtual BuddyProfile Buddy { get; set; }
        [ForeignKey("InviteId")]
        [InverseProperty("Notifications")]
        public virtual Invitation Invite { get; set; }
        [ForeignKey("OrderId")]
        [InverseProperty("Notifications")]
        public virtual Order Order { get; set; }
    }
}
