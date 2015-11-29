using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sc2iqapi.Models
{
    public class Question
    {
        [Editable(false, AllowInitialValue = false)]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Q { get; set; }

        [Required]
        [MaxLength(64)]
        public string A1 { get; set; }

        [Required]
        [MaxLength(64)]
        public string A2 { get; set; }

        [Required]
        [MaxLength(64)]
        public string A3 { get; set; }

        [Required]
        [MaxLength(64)]
        public string A4 { get; set; }

        [Required]
        public int CorrectAnswerIndex { get; set; }

        [Editable(false)]
        public DateTimeOffset Created { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        [Range(1, 10)]
        public int Difficulty { get; set; }

        public QuestionState State { get; set; }
    }

    public enum QuestionState
    {
        Pending,
        Published
    }
}
