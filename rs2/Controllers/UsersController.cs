using Microsoft.AspNetCore.Mvc;

using rs2.Models;
using rs2.Models.Database;
using rs2.Models.Repository;
using System.Collections.Generic;

namespace rs2.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private IApplicationRepostitory AppRepo { get; set; }
        private IAuthRepository AuthRepo { get; set; }

        public UsersController(IApplicationRepostitory appRepo, IAuthRepository authRepo)
        {
            AppRepo = appRepo;
            AuthRepo = authRepo;
        }

        // Admin only
        // GET api/users
        [HttpGet]
        public JsonResult Get([FromQuery]string search, [FromQuery]int offset = 0, [FromQuery]int limit = 3)
        {
            if (AuthRepo.IsAuthenticated(Role.Admin))
            {
                int count;
                var users = AppRepo.GetAllUsers(offset, limit, search, out count);
                return Json(new { Count = count, Users = users });
            }
            Response.StatusCode = 401;
            return Json(new { Msg = "Unauthorized" });
        }

        // Admin only
        // GET: api/users/2
        [HttpGet("{id:int}")]
        public JsonResult Get([FromRoute]int? id)
        {
            if (AuthRepo.IsAuthenticated(Role.Admin)) {
                if (id.HasValue)
                {
                    var user = AppRepo.GetUserModelById(id.Value);
                    if (user != null)
                        return Json(user);
                    else
                        Response.StatusCode = 404;
                        return Json(new { Msg = "User doesn't exists" });
                }
            }
            Response.StatusCode = 401;
            return Json(new { Msg = "You are not admin" });
        }

        // POST: api/users
        [HttpPost]
        public JsonResult Post([FromBody]UsersPostModel newUser)
        {
            if (ModelState.IsValid)
            {
                int status;
                string msg;
                AppRepo.AddUser(newUser, out status, out msg);
                if (status == 200)
                    return Json(new { msg = "Ok" });
                else
                    Response.StatusCode = status;
                    return Json(new { error = msg });
            }

            return Json(new { msg = "Not ok" });
        }

        // PUT api/users/5
        [HttpPut("{id:int}")]
        public void Put([FromRoute]int? id, [FromBody]ChangePasswordModel changePass)
        {
            if (id.HasValue && id.Value == -1)
            {
                if (AuthRepo.IsAuthenticated())
                {
                    if(changePass.OldPassword != null && changePass.OldPassword.Length > 0 &&
                       changePass.NewPassword != null && changePass.NewPassword.Length > 0)
                    {
                        Response.StatusCode = AppRepo.ChangePassword(AuthRepo.CurrentUserId,
                                        changePass.OldPassword, changePass.NewPassword);
                       
                        return;
                    }
                    else
                    {
                        Response.StatusCode = 401;
                        return;
                    }
                }
            }
            else if(id.HasValue && id.Value != -1)
            {
                if (AuthRepo.IsAuthenticated(Role.Admin))
                {
                    if (changePass.NewPassword != null & changePass.NewPassword.Length > 0)
                    {
                        Response.StatusCode = AppRepo.ChangePassword(id.Value, changePass.NewPassword);
                        return;
                    }
                }
                else
                {
                    Response.StatusCode = 401;
                    return;
                }
            }

            Response.StatusCode = 422;
        }

        // Admin only
        // DELETE api/users
        [HttpDelete]
        public void Delete([FromBody]IEnumerable<int> usersForDelete)
        {
            if(AuthRepo.IsAuthenticated(Role.Admin))
            {
                Response.StatusCode = AppRepo.DeleteUsers(usersForDelete);
                return;
            }

            Response.StatusCode = 401;
        }
    }
}
