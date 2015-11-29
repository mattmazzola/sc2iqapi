using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using sc2iqapi.Models;
using Microsoft.Data.Entity.Update;
using Microsoft.Data.Entity;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace sc2iqapi.Controllers
{
    [Route("api/[controller]")]
    public class QuestionsController : Controller
    {
        [FromServices]
        public Sc2IqContext DbContext { get; set; }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var questions = await DbContext.Questions
                .Include(q => q.CreatedBy)
                .Include(q => q.QuestionTags)
                .ToListAsync();

            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return Json(questions, jsonSerializerSettings);
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

            var userId = 7;
            var user = DbContext.Users.FirstOrDefault(u => u.Id == userId);
            if(user == null)
            {
                return HttpBadRequest(new Exception($"Could not find user with id: {userId}"));
            }

            question.CreatedBy = user;
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

            var questionTags = question.QuestionTags;
            foreach(var questionTag in questionTags)
            {
                questionTag.QuestionId = question.Id;
            }

            DbContext.QuestionTags.AddRange(questionTags);

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("Error", e.InnerException.Message);
                return HttpBadRequest(ModelState);
            }

            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return Json(question, jsonSerializerSettings);
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
