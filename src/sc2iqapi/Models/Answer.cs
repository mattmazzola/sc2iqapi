using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sc2iqapi.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int AnswerIndex { get; set; }
        public int Duration { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int Points { get; set; }
        public Score Score { get; set; }
    }
}
