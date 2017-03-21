using System.Collections.Generic;

using rs2.Models.Database;

namespace rs2.Models.Repository
{
    public interface IApplicationRepostitory
    {
        void AddUser(UsersPostModel newUser, out int status, out string msg);
        UserGetModel[] GetAllUsers(int limit, int offset, string search, out int count);
        UserGetModel GetUserById(int id);
        int DeleteUsers(IEnumerable<int> ids);
        string IsValidLogin(AuthLoginModel model, out User outUser, out bool status);
        string ValidateToken(string token, out bool status);
    }
}
