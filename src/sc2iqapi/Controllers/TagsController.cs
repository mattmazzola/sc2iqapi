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
    public class TagsController : Controller
    {
        [FromServices]
        public Sc2IqContext DbContext { get; set; }

        [HttpGet]
        public IActionResult Get()
        {
            var tags = DbContext.Tags;

            return Json(tags);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var tag = DbContext.Tags.FirstOrDefault(q => q.Id == id);

            if (tag == null)
            {
                return HttpNotFound();
            }

            return Json(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            tag.Created = DateTimeOffset.Now;

            DbContext.Tags.Add(tag);

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch(DbUpdateException e)
            {
                ModelState.AddModelError("Error", e.InnerException.Message);
                return HttpBadRequest(ModelState);
            }

            return Json(tag);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = DbContext.Tags.FirstOrDefault(t => t.Id == id);
            if (tag == null)
            {
                return HttpNotFound();
            }

            DbContext.Remove(tag);

            await DbContext.SaveChangesAsync();

            return Json(id);
        }
    }
}
