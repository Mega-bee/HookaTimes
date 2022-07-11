using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    [Table("Device")]
    public partial class Device
    {
        public Device()
        {
            PatientDevices = new HashSet<PatientDevice>();
        }

        [Key]
        public int Id { get; set; }
        [Column("IMEI")]
        [StringLength(450)]
        public string Imei { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        public bool? IsAssigned { get; set; }
        [StringLength(450)]
        public string CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(AspNetUser.Devices))]
        public virtual AspNetUser CreatedByNavigation { get; set; }
        [InverseProperty(nameof(PatientDevice.Device))]
        public virtual ICollection<PatientDevice> PatientDevices { get; set; }
    }
}
