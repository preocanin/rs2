using System.ComponentModel.DataAnnotations;

namespace rs2.Models
{
    public class UsersPostModel
    {
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public User ToUser()
        {
            return new User()
            {
                Username = Username,
                Email = Email,
                Password = Password
            };
        }
    }
}
