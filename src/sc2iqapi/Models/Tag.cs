using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sc2iqapi.Models
{
    public class Tag
    {
        [Editable(false, AllowInitialValue = false)]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Editable(false)]
        public DateTimeOffset Created { get; set; }

        public User CreatedBy { get; set; }

        public ICollection<QuestionTag> QuestionTags { get; set; }
    }
}
