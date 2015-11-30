using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sc2iqapi.Models
{
    public class Poll
    {
        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public User CreatedBy { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Title { get; set; }
        public int Votes { get; set; }
    }
}
