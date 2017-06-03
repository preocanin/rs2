using System.Collections.Generic;
using OfficeOpenXml;

using rs2.Models.Database;

namespace rs2.Models.Repository
{
    public interface IApplicationRepostitory
    {
        void AddUser(UsersPostModel newUser, out int status, out string msg);
        UserGetModel[] GetAllUsers(int limit, int offset, string search, out int count);
        UserGetModel GetUserModelById(int id);
        User GetUserById(int id);
        int ChangePassword(int userId, string newPassword);
        int ChangePassword(int userId, string oldPassword, string newPassword);
        int DeleteUsers(IEnumerable<int> ids);
        void AddRecord(User owner, RecordPostModel record, out int status, out string msg);
        bool AddRecordsFromExcel(User owner, ExcelWorksheet worksheet);
        int ChangeRecords(int ownerId, IEnumerable<RecordPutModel> records);
        ExcelPackage GetAllRecordsAsExcel(int ownerId);
        RecordPutModel[] GetAllRecords(int ownerId, int limit, int offset, out int count);
        int DeleteRecords(int ownerId, IEnumerable<int> ids);
        int DeleteAllRecords(int ownerId);
        string IsValidLogin(AuthLoginModel model, out User outUser, out bool status);
        string ValidateToken(string token, out bool status);
    }
}
