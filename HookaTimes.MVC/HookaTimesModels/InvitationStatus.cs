using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.MVC.HookaTimesModels
{
    [Table("InvitationStatus")]
    public partial class InvitationStatus
    {
        public InvitationStatus()
        {
            Invitations = new HashSet<Invitation>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string? Title { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [InverseProperty("InvitationStatus")]
        public virtual ICollection<Invitation> Invitations { get; set; }
    }
}
