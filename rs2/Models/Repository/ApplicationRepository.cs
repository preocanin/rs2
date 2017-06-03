using System;
using System.Security.Cryptography;
using System.Linq;
using System.Collections.Generic;
using OfficeOpenXml;

using Jose;
using rs2.Models.Database;

namespace rs2.Models.Repository
{
    public class ApplicationRepository : IApplicationRepostitory
    {
        public ApplicationRepository(AppDbContext context)
        {
            Context = context;

            // Use only in development
            Context.Database.EnsureCreated();
        }

        public void AddUser(UsersPostModel u, out int statusCode, out string msg)
        {
            User newUser = u.ToUser();
            newUser.Role = Role.Client;
            byte[] salt = MakeSalt();
            newUser.Salt = Convert.ToBase64String(salt);
            newUser.Password = HashPassword(u.Password, salt);
            Context.Users.Add(newUser);
            try
            {
                Context.SaveChanges();
            }
            catch (Exception)
            {
                statusCode = 400;
                msg = "User already exists";
                return;
            }
            statusCode = 200;
            msg = "Ok";
        }

        public UserGetModel[] GetAllUsers(int offset, int limit, string search, out int count)
        {
            var users =  from u in Context.Users
                         let searchResult = 
                             search == null? true : (u.Username.Contains(search) || u.Email.Contains(search))
                         where u.Role != Role.Admin && 
                               searchResult
                         select new UserGetModel()
                         {
                             UserId = u.UserId,
                             Username = u.Username
                         };

            count = users.Count();
            return users.Skip(offset).Take(limit).ToArray();
        }

        public UserGetModel GetUserModelById(int id)
        {
            var users = from u in Context.Users
                        where u.UserId == id
                        select new UserGetModel()
                        {
                            UserId = u.UserId,
                            Username = u.Username
                        };
            return users == null ? null : users.Count() == 0? null : users.First();
        }

        public User GetUserById(int id)
        {
            var users = from u in Context.Users
                        where u.UserId == id
                        select u;
            return users == null ? null : users.Count() == 0? null : users.First();
        }

        public int ChangePassword(int userId, string newPassword)
        {
            User user = GetUserById(userId);
            if(user != null)
            {
                byte[] salt = Convert.FromBase64String(user.Salt);
                user.Password = HashPassword(newPassword, salt);

                try
                {
                    Context.SaveChanges();
                    return 200;
                }
                catch (Exception)
                {
                    return 500;
                }
            }
            return 500;
        }

        public int ChangePassword(int userId, string oldPassword, string newPassword)
        {
            User curr_user = GetUserById(userId);
            byte[] salt = Convert.FromBase64String(curr_user.Salt);
            oldPassword = HashPassword(oldPassword, salt);   
            if(oldPassword == curr_user.Password)
            {
                curr_user.Password = HashPassword(newPassword, salt);
                try
                {
                    Context.SaveChanges();
                    return 200;
                }
                catch(Exception)
                {
                    return 500;
                }
            }
            return 401;
        }

        public int DeleteUsers(IEnumerable<int> ids)
        {
            var users = from u in Context.Users
                        where u.Role == Role.Client &&
                              ids.Contains(u.UserId)
                        select u;
            if (users == null || users.Count() == 0)
                return 400;
            else
            {
                Context.Users.RemoveRange(users);
                Context.SaveChanges();
                return 200;
            }
        }

        public void AddRecord(User owner, RecordPostModel recordPost, out int statusCode, out string msg)
        {
            Record newRecord = recordPost.toRecord(owner);
            Context.Records.Add(newRecord);
            try
            {
                Context.SaveChanges();
            }
            catch(Exception) 
            {
                statusCode = 400;
                msg = "Record already exists";
                return;
            }

            statusCode = 200;
            msg = "ok";
        }

        public bool AddRecordsFromExcel(User owner, ExcelWorksheet worksheet) {
            if (worksheet.Dimension.End.Row > 0 
                && worksheet.Dimension.End.Column > 0
                && worksheet.Dimension.End.Column == 4
                )
            {
                int rstart = worksheet.Dimension.Start.Row;
                if(TestFirstRowForNames(worksheet))
                {
                    return InsertToDatabase(owner, worksheet, rstart + 1);
                }
                else
                {
                    return InsertToDatabase(owner, worksheet, rstart);
                }
            }
            else
            {
                return false;
            }
        }

        public ExcelPackage GetAllRecordsAsExcel(int ownerId)
        {
            User owner = GetUserById(ownerId);
            ExcelPackage package = new ExcelPackage();
            package.Workbook.Properties.Title = "Records";
            package.Workbook.Properties.Author = owner.Username;

            var worksheet = package.Workbook.Worksheets.Add("Records");

            //First add headers
            worksheet.Cells[1, 1].Value = "Bx";
            worksheet.Cells[1, 2].Value = "By";
            worksheet.Cells[1, 3].Value = "Ax";
            worksheet.Cells[1, 4].Value = "Ay";

            var records = from r in Context.Records
                          where r.User.UserId == ownerId
                          select r;

            if(records != null)
            {
                int i = 2;
                foreach(var r in records)
                {
                    worksheet.Cells[i, 1].Value = r.BeforeX;
                    worksheet.Cells[i, 2].Value = r.BeforeY;
                    worksheet.Cells[i, 3].Value = r.AfterX;
                    worksheet.Cells[i, 4].Value = r.AfterY;
                    i++;
                }
            }

            return package;
        }

        public int ChangeRecords(int ownerId, IEnumerable<RecordPutModel> records)
        {
            var userRecords = from r in Context.Records
                              let recordIds = records.Select(x => x.RecordId)
                              where r.User.UserId == ownerId &&
                                    recordIds.Contains(r.RecordId)
                              select new { Record = r, NewData = records.Single(x => x.RecordId == r.RecordId) };

            if (userRecords == null || userRecords.Count() == 0)
                return 403;
            else
            {
                foreach(var pair in userRecords)
                {
                    pair.Record.BeforeX = pair.NewData.Bx;
                    pair.Record.BeforeY = pair.NewData.By;
                    pair.Record.AfterX = pair.NewData.Ax;
                    pair.Record.AfterY = pair.NewData.Ay;
                }

                Context.SaveChanges();
                return 200;
            }
        }

        public RecordPutModel[] GetAllRecords(int ownerId, int limit, int offset, out int count)
        {
            var records = from r in Context.Records
                          where r.User.UserId == ownerId
                          select new RecordPutModel()
                          {
                              RecordId = r.RecordId,
                              Bx = r.BeforeX,
                              By = r.BeforeY,
                              Ax = r.AfterX,
                              Ay = r.AfterY
                          };

            count = records.Count();
            return records.Skip(offset).Take(limit).ToArray();
        }

        public int DeleteRecords(int ownerId, IEnumerable<int> ids)
        {
            var records = from r in Context.Records
                          where r.User.UserId == ownerId &&
                                ids.Contains(r.RecordId)
                          select r;
            if (records == null || records.Count() == 0)
                return 403;
            else
            {
                Context.Records.RemoveRange(records);
                Context.SaveChanges();
                return 200;
            }
        }

        public int DeleteAllRecords(int ownerId)
        {
            var records = from r in Context.Records
                          where r.User.UserId == ownerId
                          select r;
            if (records == null || records.Count() == 0)
                return 403;
            else
            {
                Context.Records.RemoveRange(records);
                Context.SaveChanges();
                return 200;
            }
        }

        public string IsValidLogin(AuthLoginModel model, out User outUser, out bool status)
        {
            var user = (from u in Context.Users
                        where u.Email == model.Email
                        select u).First();

            outUser = user;

            if (CheckPassowrd(model.Password, user))
            {
                status = true;
                return EncodeToken(new Dictionary<string, object>
                {
                    { "userId", user.UserId },
                    { "username", user.Username },
                    { "role", user.Role }
                });
            }

            status = false;
            return "";
        }

        public string ValidateToken(string token, out bool status)
        {
            try
            {
                status = true;
                return JWT.Decode(token, Secret, JwsAlgorithm.HS256);
            }
            catch(Exception)
            {
                status = false;
                return "";
            }
        }

        private string EncodeToken(Dictionary<string, object> payload)
        {
            return JWT.Encode(payload, Secret, JwsAlgorithm.HS256);
        }

        private bool CheckPassowrd(string password, User u)
        {
            return
               HashPassword(password, Convert.FromBase64String(u.Salt)) == u.Password;
        }

        private string HashPassword(string password, byte[] salt, int hashByteSize = 24) {
            using (var hashGenerator = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                return Convert.ToBase64String(hashGenerator.GetBytes(hashByteSize));
            }
        }

        private byte[] MakeSalt(int byteSize = 16)
        {
            byte[] salt = new byte[byteSize];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private AppDbContext Context { get; set; }

        private byte[] Secret {
            get {
                return new byte[]
                    {
                        0x55,0x71,0x74,0x36,0x79,0x44,0x72,0x2f,0x70,0x39,0x73,0x75,0x52,0x42,0x63,0x36,
                        0x6d,0x48,0x72,0x47,0x69,0x57,0x68,0x47,0x70,0x55,0x4e,0x61,0x33,0x45,0x62,0x33,
                        0x37,0x63,0x59,0x57,0x45,0x47,0x4f,0x52,0x4e,0x4e,0x6a,0x71,0x38,0x59,0x6e,0x59,
                        0x39,0x54,0x48,0x66,0x56,0x65,0x77,0x6c,0x2f,0x63,0x63,0x4b,0x37,0x4c,0x78,0x34,0x22
                    };
                    //"zAH2zpxtTXyqhCGS"
            }
        }

        /*
         * Check if first row contatins names ax, ay, bx, by
         */
        private bool TestFirstRowForNames(ExcelWorksheet worksheet)
        {
            List<string> colNames = new List<string>(4)
            {
                "ax", "Ax", "aX", "AX",
                "bx", "Bx", "bX", "BX",
                "ay", "Ay", "aY", "AY",
                "by", "By", "bY", "BY"
            };

            int firstRow = worksheet.Dimension.Start.Row;
            int start = worksheet.Dimension.Start.Column;
            if(colNames.Contains(worksheet.Cells[firstRow, start].Value.ToString()) &&
                colNames.Contains(worksheet.Cells[firstRow, start + 1].Value.ToString()) &&
                colNames.Contains(worksheet.Cells[firstRow, start + 2].Value.ToString()) &&
                colNames.Contains(worksheet.Cells[firstRow, start + 3].Value.ToString()))
            {
                return true; 
            }
            return false;
        }

        private bool InsertToDatabase(User owner, ExcelWorksheet worksheet, int rstart)
        {
            for(int i = rstart;
                i <= worksheet.Dimension.End.Row; 
                ++i)
            {
                int cstart = worksheet.Dimension.Start.Column;
                try
                {
                    Context.Records.Add(new Record()
                    {
                        BeforeX = (float)Convert.ToDouble(worksheet.Cells[i, cstart].Value.ToString()),
                        BeforeY = (float)Convert.ToDouble(worksheet.Cells[i, cstart + 1].Value.ToString()),
                        AfterX = (float)Convert.ToDouble(worksheet.Cells[i, cstart + 2].Value.ToString()),
                        AfterY = (float)Convert.ToDouble(worksheet.Cells[i, cstart + 3].Value.ToString()),
                        User = owner
                    });
                }
                catch(Exception)
                {
                    return false;
                }
            }

            Context.SaveChanges();
            return true;
        }
    }
}
