using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HookaTimes.DAL.Models
{
    [Table("Medicine")]
    public partial class Medicine
    {
        public Medicine()
        {
            PatientMedicines = new HashSet<PatientMedicine>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(250)]
        public string Title { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        public bool? IsDeleted { get; set; }

        [InverseProperty(nameof(PatientMedicine.Medicine))]
        public virtual ICollection<PatientMedicine> PatientMedicines { get; set; }
    }
}
