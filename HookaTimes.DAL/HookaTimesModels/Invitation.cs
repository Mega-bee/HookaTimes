using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("Invitation")]
    public partial class Invitation
    {
        [Key]
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? InvitationStatusId { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? InvitationDate { get; set; }
        public int? InvitationOptionId { get; set; }
        public int FromBuddyId { get; set; }
        public int? ToBuddyId { get; set; }
        public int PlaceId { get; set; }

        [ForeignKey("FromBuddyId")]
        [InverseProperty("InvitationFromBuddies")]
        public virtual BuddyProfile FromBuddy { get; set; }
        [ForeignKey("InvitationOptionId")]
        [InverseProperty("Invitations")]
        public virtual InvitationOption InvitationOption { get; set; }
        [ForeignKey("InvitationStatusId")]
        [InverseProperty("Invitations")]
        public virtual InvitationStatus InvitationStatus { get; set; }
        [ForeignKey("ToBuddyId")]
        [InverseProperty("InvitationToBuddies")]
        public virtual BuddyProfile ToBuddy { get; set; }
    }
}
