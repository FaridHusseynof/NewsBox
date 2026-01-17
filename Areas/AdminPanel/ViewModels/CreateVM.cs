using System.ComponentModel.DataAnnotations;

namespace NewsBox.Areas.AdminPanel.ViewModels
{
    public class CreateVM
    {
        [Required]
        public string title { get; set; }
        [Required]
        public string author { get; set; }
        public string? imageURL { get; set; }
        public IFormFile? imageFile { get; set; }
    }
}
