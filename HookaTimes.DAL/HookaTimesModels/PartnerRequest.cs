using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HookaTimes.DAL.HookaTimesModels
{
    [Table("PartnerRequest")]
    public partial class PartnerRequest
    {
        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string FirstName { get; set; }
        [StringLength(255)]
        public string LastName { get; set; }
        [StringLength(255)]
        public string PhoneNumber { get; set; }
        [StringLength(255)]
        public string EmailAddress { get; set; }
        [StringLength(255)]
        public string HookaPlaceName { get; set; }
        [StringLength(450)]
        public string Description { get; set; }
        [StringLength(255)]
        public string Location { get; set; }
    }
}
