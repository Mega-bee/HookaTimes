using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    public partial class AspNetUserToken
    {
        [Key]
        public string UserId { get; set; }
        [Key]
        public string LoginProvider { get; set; }
        [Key]
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
