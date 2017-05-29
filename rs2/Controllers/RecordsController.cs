using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using rs2.Models;
using rs2.Models.Database;
using rs2.Models.Repository;

namespace rs2.Controllers
{
    [Route("api/[controller]")]
    public class RecordsController : Controller
    {
        private IApplicationRepostitory AppRepo { get; set; }
        private IAuthRepository AuthRepo { get; set; }

        public RecordsController(IApplicationRepostitory appRepo, IAuthRepository authRepo)
        {
            AppRepo = appRepo;
            AuthRepo = authRepo;
        }

        // GET: api/records
        [HttpGet]
        public JsonResult Get([FromQuery]int offset = 0, [FromQuery]int limit = 3)
        {
            if(AuthRepo.IsAuthenticated())
            {
                int count;
                var records = AppRepo.GetAllRecords(
                    AuthRepo.CurrentUserId, limit, offset, out count);
                return Json(new { Count = count, Records = records });
            }
            Response.StatusCode = 401;
            return Json(new { Msg = "Unauthorized" });
        }

        // GET: api/records/5
        //[HttpGet("{id}")]
        //public Record Get(int id)
        //{
        //    //Jedan record for CURRENT_USER
        //    return "value";
        //}

        // POST: api/records
        [HttpPost]
        public JsonResult Post([FromBody]RecordPostModel recordModel)
        {
            // Admin and Client can access
            if(AuthRepo.IsAuthenticated())
            {
                User currentUser = AppRepo.GetUserById(AuthRepo.CurrentUserId);
                int status;
                string msg;
                AppRepo.AddRecord(currentUser, recordModel, out status, out msg);
                Response.StatusCode = status;
                return Json(new { Msg = msg });
            }
            Response.StatusCode = 401;
            return Json(new { Msg = "Unauthorized" });
        }

        // PUT: api/records/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            //Change record ADMIN and CURRENT_USER
        }

        // DELETE: api/records/5
        [HttpDelete]
        public void Delete([FromBody]IEnumerable<int> recordsForDelete)
        {
            if(AuthRepo.IsAuthenticated())
            {
                Response.StatusCode = AppRepo.DeleteRecords(AuthRepo.CurrentUserId, recordsForDelete);
                return;
            }
            Response.StatusCode = 401;
        }
    }
}
