using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sc2iqapi.Models
{
    public class Sc2IqContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
