using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("Location")]
    public partial class Location
    {
        public Location()
        {
            PlacesProfiles = new HashSet<PlacesProfile>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [InverseProperty("Location")]
        public virtual ICollection<PlacesProfile> PlacesProfiles { get; set; }
    }
}
