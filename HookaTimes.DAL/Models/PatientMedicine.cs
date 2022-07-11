using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    [Table("PatientMedicine")]
    public partial class PatientMedicine
    {
        [Key]
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int? MedicineId { get; set; }

        [ForeignKey(nameof(MedicineId))]
        [InverseProperty("PatientMedicines")]
        public virtual Medicine Medicine { get; set; }
        [ForeignKey(nameof(PatientId))]
        [InverseProperty("PatientMedicines")]
        public virtual Patient Patient { get; set; }
    }
}
