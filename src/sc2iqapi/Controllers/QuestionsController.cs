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
    public class QuestionsController : Controller
    {
        [FromServices]
        public Sc2IqContext DbContext { get; set; }

        [HttpGet]
        public IActionResult Get()
        {
            var questions = DbContext.Questions;

            return Json(questions);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question = DbContext.Questions.FirstOrDefault(q => q.Id == id);

            if(question == null)
            {
                return HttpNotFound();
            }

            return Json(question);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Question question)
        {
            if(!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            question.Created = DateTimeOffset.Now;

            DbContext.Questions.Add(question);

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("Error", e.InnerException.Message);
                return HttpBadRequest(ModelState);
            }

            return Json(question);
        }

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
