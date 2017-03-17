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

        //TODO: User(id)
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
                return Convert.FromBase64String("Uqt6yDr/p9suRBc6");
            }
        }
    }
}
