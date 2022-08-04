using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("InvitationOption")]
    public partial class InvitationOption
    {
        public InvitationOption()
        {
            Invitations = new HashSet<Invitation>();
        }

        [Key]
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(255)]
        public string Title { get; set; }

        [InverseProperty("InvitationOption")]
        public virtual ICollection<Invitation> Invitations { get; set; }
    }
}
