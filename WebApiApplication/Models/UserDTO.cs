using System.ComponentModel.DataAnnotations;
using WebApiApplication.Helpers;

namespace WebApiApplication.Models
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [FirstCapitalLetter]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        public string Role { get; set; }

        public string Password { get; set; }
    }
}
