using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    public partial class Gender
    {
        public Gender()
        {
            AccProfiles = new HashSet<AccProfile>();
            AspNetUsers = new HashSet<AspNetUser>();
            Patients = new HashSet<Patient>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [InverseProperty(nameof(AccProfile.Gender))]
        public virtual ICollection<AccProfile> AccProfiles { get; set; }
        [InverseProperty(nameof(AspNetUser.Gender))]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        [InverseProperty(nameof(Patient.Gender))]
        public virtual ICollection<Patient> Patients { get; set; }
    }
}
