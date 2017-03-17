using System.Collections.Generic;

using rs2.Models.Database;

namespace rs2.Models.Repository
{
    public interface IApplicationRepostitory
    {
        void AddUser(UsersPostModel newUser, out int status, out string msg);
        UserGetModel[] GetAllUsers(int limit, int offset, out int count);
        UserGetModel GetUserById(int id);
        int DeleteUser(int id);
        string IsValidLogin(AuthLoginModel model, out User outUser, out bool status);
        string ValidateToken(string token, out bool status);
    }
}
