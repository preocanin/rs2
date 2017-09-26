using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using rs2.Models;
using rs2.Models.Repository;
using rs2.Models.Database;

namespace rs2.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private IApplicationRepostitory AppRepo { get; set; }
        private IAuthRepository AuthRepo { get; set; }

        public AuthController(IApplicationRepostitory appRepo, IAuthRepository authRepo)
        {
            AppRepo = appRepo;
            AuthRepo = authRepo;
        }

        // POST - api/auth/login
        [HttpPost("login")]
        public JsonResult Post([FromBody]AuthLoginModel data)
        {
            if(ModelState.IsValid)
            {
                bool status;
                User user;
                string access_token = AppRepo.IsValidLogin(data, out user, out status);
                if (status)
                {
                    CookieOptions options = new CookieOptions()
                    {
                        Expires = DateTime.Now.AddYears(1)
                    };
                    Response.Cookies.Append("access_token", access_token, options);
                    return Json(new { UserId = user.UserId, Role = user.Role });
                }
            }
            Response.StatusCode = 401;
            return Json(new { error = "Unauthorized Error" });
        }

        // POST - api/auth/logout
        [HttpPost("logout")]
        public void Post()
        {
            if (AuthRepo.IsAuthenticated())
                Response.Cookies.Delete("access_token");
        }
    }
}
