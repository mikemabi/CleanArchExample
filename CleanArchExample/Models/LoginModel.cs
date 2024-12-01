using System.ComponentModel.DataAnnotations;

namespace CleanArchExample.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
