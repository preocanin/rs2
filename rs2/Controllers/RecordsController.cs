using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

using rs2.Models;
using rs2.Models.Database;
using rs2.Models.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace rs2.Controllers
{
    [Route("api/[controller]")]
    public class RecordsController : Controller
    {
        private IApplicationRepostitory AppRepo { get; set; }
        private IAuthRepository AuthRepo { get; set; }

        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

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

        // POST: api/records/file
        [HttpPost("file")]
        public void Post(IFormFile file)
        {
            if (AuthRepo.IsAuthenticated())
            {

                if (file != null && file.ContentType == XlsxContentType)
                {
                    var filename = Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ".xlsx";
                    var filePath = Path.Combine(Path.GetTempPath(), filename);

                    if (file.Length > 0)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                        {
                            file.CopyTo(stream);
                            stream.Flush();
                        }
                    }

                    FileStream streamNew = null;
                    try
                    {
                        streamNew = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                    }
                    catch (Exception)
                    {
                        Response.StatusCode = 500;
                        return;
                    }

                    ExcelPackage excelFile = new ExcelPackage(streamNew);
                    ExcelWorksheet worksheet = excelFile.Workbook.Worksheets[1];

                    if (!AppRepo.AddRecordsFromExcel(AppRepo.GetUserById(AuthRepo.CurrentUserId), worksheet))
                        Response.StatusCode = 500;

                    return; 
                }
            }

            Response.StatusCode = 401;
        }

        // GET: api/records/file
        [HttpGet("file")]
        public IActionResult Get()
        {
            if(AuthRepo.IsAuthenticated())
            {
                var package = AppRepo.GetAllRecordsAsExcel(AuthRepo.CurrentUserId);
                return File(package.GetAsByteArray(), XlsxContentType, "records.xlsx");
            }

            Response.StatusCode = 401;
            return Json(new { Msg = "Unauthorized " });
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
