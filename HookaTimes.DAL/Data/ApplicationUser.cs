using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HookaTimes.DAL.Data
{
    public class ApplicationUser : IdentityUser
    {

        public string FcmToken { get; set; }
        public int? GenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsDeleted { get; set; }
        public string Image { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
