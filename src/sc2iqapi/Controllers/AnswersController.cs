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
    public class AnswersController : Controller
    {
        [FromServices]
        public Sc2IqContext DbContext { get; set; }

        [HttpGet]
        public IActionResult Get()
        {
            var answers = DbContext.Answers;

            return Json(answers);
        }

        [HttpGet("{id}")]
        public IActionResult Get(ICollection<int> ids)
        {
            var answers = DbContext.Answers
                .Where(x => ids.Contains(x.Id));

            return Json(answers);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ICollection<Answer> answers)
        {
            // Validate Model
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            // Get current user
            var userId = 7;
            var user = DbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return HttpBadRequest(new Exception($"Could not find user with id: {userId}"));
            }

            // Compute Points per Answer
            foreach (var answer in answers)
            {
                var question = DbContext.Questions.FirstOrDefault(q => q.Id == answer.QuestionId);
                if (question == null)
                {
                    return HttpBadRequest(new Exception($"Could not find question with id: {answer.QuestionId}. Likely that answer was submitted with invalid data."));
                }

                answer.Question = question;
                // TODO: Replace with calculation based on question difficulty / duration
                answer.Points = 10;
            }

            // Create total Score
            var score = new Score()
            {
                User = user,
                Points = answers.Aggregate(0, (total, answer) => { return total + answer.Points; }),
                Duration = answers.Aggregate(0, (total, answer) => { return total + answer.Duration; }),
                Submitted = DateTimeOffset.Now
            };

            // Save Score
            DbContext.Scores.Add(score);

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("Error", e.InnerException.Message);
                return HttpBadRequest(ModelState);
            }

            // Add score to each answer
            foreach (var answer in answers)
            {
                answer.Score = score;
            }

            // Save answers
            DbContext.Answers.AddRange(answers);

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("Error", e.InnerException.Message);
                return HttpBadRequest(ModelState);
            }

            return Json(score);
        }
    }
}
