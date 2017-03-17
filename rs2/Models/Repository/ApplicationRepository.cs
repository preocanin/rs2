using System;
using System.Security.Cryptography;
using System.Linq;
using System.Collections.Generic;

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
                statusCode = 422;
                msg = "User already exists";
                return;
            }
            statusCode = 200;
            msg = "Ok";
        }

        public UserGetModel[] GetAllUsers(int offset, int limit, out int count)
        {
            var users =  from u in Context.Users
                         where u.Role != Role.Admin
                         select new UserGetModel()
                         {
                             UserId = u.UserId,
                             Username = u.Username
                         };

            count = users.Count();
            return users.Skip(offset).Take(limit).ToArray();
        }

        public UserGetModel GetUserById(int id)
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

        public int DeleteUser(int id)
        {
            var users = from u in Context.Users
                       where u.UserId == id &&
                             u.Role == Role.Client
                       select u;
            if (users == null || users.Count() == 0)
                return 404;
            else
            {
                var user = users.First();
                Context.Users.Remove(user);
                Context.SaveChanges();
                return 200;
            }
        }

        //TODO: AllRecords()

        //TODO: Record(id)

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
    }
}
