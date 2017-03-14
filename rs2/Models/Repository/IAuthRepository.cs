using rs2.Models.Database;

namespace rs2.Models.Repository
{
    public interface IAuthRepository
    {
        bool IsAuthenticated(Role role = Role.Client);
        int CurrentUserId { get; set; }
        Role CurrentUserRole { get; set; }
    }
}
