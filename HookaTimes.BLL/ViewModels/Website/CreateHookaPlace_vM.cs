using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookaTimes.BLL.ViewModels.Website
{
    public class CreateHookaPlace_vM
    {
      
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int CuisineId { get; set; }
        [Required]
        public float Rating { get; set; }
        [Required]
        public string OpeningFrom { get; set; }
        [Required]
        public string OpeningTo { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Longitude { get; set; }
        [Required]
        public string Latitude { get; set; }
        [Required]
        public IFormFileCollection Albums { get; set; }
        [Required]
        public IFormFileCollection Menus { get; set; }

    }
}
