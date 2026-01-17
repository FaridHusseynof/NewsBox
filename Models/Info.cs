using System.ComponentModel.DataAnnotations;

namespace NewsBox.Models
{
    public class Info: BaseModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        public string ImageURL { get; set; }
    }
}
