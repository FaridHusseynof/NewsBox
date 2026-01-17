using System.ComponentModel.DataAnnotations;

namespace NewsBox.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage ="Your passwords doesn't match")]
        public string CheckPassword { get; set; }
    }
}
