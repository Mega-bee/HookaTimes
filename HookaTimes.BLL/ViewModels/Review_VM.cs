using System.ComponentModel.DataAnnotations;

namespace HookaTimes.BLL.ViewModels
{
    public partial class CreateReview_VM
    {
        [Required]
        public float Rating { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
