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
    public class UsersController : Controller
    {
        [FromServices]
        public Sc2IqContext DbContext { get; set; }

        [HttpGet]
        public IActionResult Get()
        {
            var users = DbContext.Users;

            return Json(users);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = DbContext.Users.FirstOrDefault(q => q.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return Json(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody][Bind("BattleNetId")]User user)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            user.PointsEarned = 0;
            user.PointsSpent = 0;
            user.Role = UserRole.User;
            user.Reputation = 0;
            user.Created = DateTimeOffset.Now;

            DbContext.Users.Add(user);

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("Error", e.InnerException.Message);
                return HttpBadRequest(ModelState);
            }

            return Json(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = DbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            DbContext.Remove(user);

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("Error", e.InnerException.Message);
                return HttpBadRequest(ModelState);
            }

            return Json(id);
        }
    }
}
