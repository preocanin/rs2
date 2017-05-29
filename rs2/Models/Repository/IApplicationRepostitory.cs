using System.Collections.Generic;

using rs2.Models.Database;

namespace rs2.Models.Repository
{
    public interface IApplicationRepostitory
    {
        void AddUser(UsersPostModel newUser, out int status, out string msg);
        UserGetModel[] GetAllUsers(int limit, int offset, string search, out int count);
        UserGetModel GetUserModelById(int id);
        User GetUserById(int id);
        int DeleteUsers(IEnumerable<int> ids);
        void AddRecord(User owner, RecordPostModel record, out int status, out string msg);
        RecordPostModel[] GetAllRecords(int ownerId, int limit, int offset, out int count);
        int DeleteRecords(int ownerId, IEnumerable<int> ids);
        string IsValidLogin(AuthLoginModel model, out User outUser, out bool status);
        string ValidateToken(string token, out bool status);
    }
}
