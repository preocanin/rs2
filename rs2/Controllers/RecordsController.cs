using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;


namespace rs2.Controllers
{
    [Route("api/[controller]")]
    public class RecordsController : Controller
    {
        // GET: api/records
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //All records for CURRENT_USER
            return new string[] { "value1", "value2" };
        }

        // GET: api/records/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //Jedan record for CURRENT_USER
            return "value";
        }

        // POST: api/records
        [HttpPost]
        public void Post([FromBody]string value)
        {
            //Post records
        }

        // PUT: api/records/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            //Change record ADMIN and CURRENT_USER
        }

        // DELETE: api/records/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //Delete record ADMIN and CURRENT_USER
        }
    }
}
