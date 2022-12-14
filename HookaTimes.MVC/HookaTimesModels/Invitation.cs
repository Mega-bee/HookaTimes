using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.MVC.HookaTimesModels
{
    [Table("Invitation")]
    public partial class Invitation
    {
        public Invitation()
        {
            Notifications = new HashSet<Notification>();
        }

        [Key]
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? InvitationStatusId { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? InvitationDate { get; set; }
        public int? InvitationOptionId { get; set; }
        public int? FromBuddyId { get; set; }
        public int? ToBuddyId { get; set; }
        public int? PlaceId { get; set; }

        [ForeignKey("FromBuddyId")]
        [InverseProperty("InvitationFromBuddies")]
        public virtual BuddyProfile? FromBuddy { get; set; }
        [ForeignKey("InvitationOptionId")]
        [InverseProperty("Invitations")]
        public virtual InvitationOption? InvitationOption { get; set; }
        [ForeignKey("InvitationStatusId")]
        [InverseProperty("Invitations")]
        public virtual InvitationStatus? InvitationStatus { get; set; }
        [ForeignKey("PlaceId")]
        [InverseProperty("Invitations")]
        public virtual PlacesProfile? Place { get; set; }
        [ForeignKey("ToBuddyId")]
        [InverseProperty("InvitationToBuddies")]
        public virtual BuddyProfile? ToBuddy { get; set; }
        [InverseProperty("Invite")]
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
