using System.ComponentModel.DataAnnotations;

namespace rs2.Models
{
    public class AuthLoginModel
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
