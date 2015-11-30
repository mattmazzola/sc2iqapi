using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using sc2iqapi.Models;
using Microsoft.Data.Entity.Update;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace sc2iqapi.Controllers
{
    [Route("api/[controller]")]
    public class ScoresController : Controller
    {
        [FromServices]
        public Sc2IqContext DbContext { get; set; }

        [HttpGet]
        public IActionResult Get()
        {
            var scores = DbContext.Scores;

            return Json(scores);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var scores = DbContext.Scores.FirstOrDefault(x => x.Id == id);

            if (scores == null)
            {
                return HttpNotFound();
            }

            return Json(scores);
        }
    }
}
