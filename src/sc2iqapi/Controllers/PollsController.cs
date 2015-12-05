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
    public class PollsController : Controller
    {
        [FromServices]
        public Sc2IqContext DbContext { get; set; }

        [HttpGet]
        public IActionResult Get()
        {
            var polls = DbContext.Polls;

            return Json(polls);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var poll = DbContext.Polls.FirstOrDefault(q => q.Id == id);

            if (poll == null)
            {
                return HttpNotFound();
            }

            return Json(poll);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Poll poll)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            var userId = 7;
            var user = DbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return HttpBadRequest(new Exception($"Could not find user with id: {userId}"));
            }

            poll.CreatedBy = user;
            poll.Created = DateTimeOffset.Now;
            poll.Votes = 0;

            DbContext.Polls.Add(poll);

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("Error", e.InnerException.Message);
                return HttpBadRequest(ModelState);
            }

            return Json(poll);
        }

        // PUT api/values/5
        [HttpPut("{id}/vote")]
        public async Task<IActionResult> Put(int id, [FromQuery]bool down)
        {
            var poll = DbContext.Polls.FirstOrDefault(q => q.Id == id);

            if (poll == null)
            {
                return HttpNotFound();
            }

            if(down)
            {
                poll.Votes -= 1;
            }
            else
            {
                poll.Votes += 1;
            }

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("Error", e.InnerException.Message);
                return HttpBadRequest(ModelState);
            }

            return Json(poll);
        }
    }
}
