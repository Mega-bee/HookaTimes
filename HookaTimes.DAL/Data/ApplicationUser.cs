using Microsoft.AspNetCore.Identity;
using System;

namespace HookaTimes.DAL.Data
{
    public class ApplicationUser : IdentityUser
    {

        public string FcmToken { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool? IsDeleted { get; set; }

    }
}
