using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.MVC.HookaTimesModels
{
    [Table("PlaceAlbum")]
    public partial class PlaceAlbum
    {
        [Key]
        public int Id { get; set; }
        public int? PlaceProfileId { get; set; }
        [StringLength(255)]
        public string? Image { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("PlaceProfileId")]
        [InverseProperty("PlaceAlbums")]
        public virtual PlacesProfile? PlaceProfile { get; set; }
    }
}
