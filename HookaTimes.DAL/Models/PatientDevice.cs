using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    public partial class PatientDevice
    {
        [Key]
        public int Id { get; set; }
        public int? DeviceId { get; set; }
        public int? PatientId { get; set; }
        public bool? IsActive { get; set; }

        [ForeignKey(nameof(DeviceId))]
        [InverseProperty("PatientDevices")]
        public virtual Device Device { get; set; }
        [ForeignKey(nameof(PatientId))]
        [InverseProperty("PatientDevices")]
        public virtual Patient Patient { get; set; }
    }
}
