using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("PlaceFilter")]
    public partial class PlaceFilter
    {
        [Key]
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
    }
}
