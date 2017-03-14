using Microsoft.AspNetCore.Mvc;

using rs2.Models;
using rs2.Models.Repository;

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

        // GET api/users
        [HttpGet]
        public JsonResult Get()
        {
            //All users
            return Json(AppRepo.AllUsers);
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            // Just for ADMIN
            if (AuthRepo.IsAuthenticated())
                return Json(new { msg = "Authenticated" });
            return Json(new { msg = "Unauthorized" });
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
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            //Change pasword ALLUSERS
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //Delete user ADMIN
        }
    }
}
