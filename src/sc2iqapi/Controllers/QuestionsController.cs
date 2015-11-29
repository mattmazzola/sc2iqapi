using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using sc2iqapi.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace sc2iqapi.Controllers
{
    [Route("api/[controller]")]
    public class QuestionsController : Controller
    {
        [FromServices]
        public Sc2IqContext DbContext { get; set; }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            var questions = DbContext.Questions;

            return Json(questions);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question = DbContext.Questions.FirstOrDefault(q => q.Id == id);

            return Json(question);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Question question)
        {
            if(!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            DbContext.Questions.Add(question);
            await DbContext.SaveChangesAsync();

            return Json(question);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var question = DbContext.Questions.FirstOrDefault(q => q.Id == id);
            if(question == null)
            {
                return HttpNotFound();
            }

            DbContext.Remove(question);

            await DbContext.SaveChangesAsync();

            return Json(id);
        }
    }
}
