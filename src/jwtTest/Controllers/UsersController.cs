using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Net;
using jwtTest.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace jwtTest.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Json(new string[] { "value1", "value2" });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new HttpNotFoundResult();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            return Json(user);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]User user)
        {
            return new HttpStatusCodeResult((int)HttpStatusCode.Accepted);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return new HttpStatusCodeResult((int)HttpStatusCode.NoContent);
        }
    }
}
