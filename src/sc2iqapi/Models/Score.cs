using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sc2iqapi.Models
{
    public class Score
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int Points { get; set; }
        public int Duration { get; set; }
        public DateTimeOffset Submitted { get; set; }
    }
}
