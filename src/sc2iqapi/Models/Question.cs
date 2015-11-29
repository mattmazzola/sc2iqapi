using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sc2iqapi.Models
{
    public class Question
    {
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
        public int Difficulty { get; set; }

        [Required]
        public int CorrectAnswerIndex { get; set; }

        public QuestionState State { get; set; }

        [Required]
        public int CreatedBy { get; set; }
    }

    public enum QuestionState
    {
        Pending,
        Published
    }
}
