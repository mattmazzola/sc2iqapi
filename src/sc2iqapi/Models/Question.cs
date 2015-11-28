using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sc2iqapi.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Q { get; set; }
        public string A1 { get; set; }
        public string A2 { get; set; }
        public string A3 { get; set; }
        public string A4 { get; set; }
        public int Difficulty { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public QuestionState State { get; set; }
        public int CreatedBy { get; set; }
    }

    public enum QuestionState
    {
        Pending,
        Published
    }
}
