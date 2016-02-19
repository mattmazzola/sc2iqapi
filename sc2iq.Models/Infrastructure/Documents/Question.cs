using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Documents
{
    public class Question
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Q { get; set; }
        public string A1 { get; set; }
        public string A2 { get; set; }
        public string A3 { get; set; }
        public string A4 { get; set; }

        public int CorrectAnswerIndex { get; set; }
        public DateTimeOffset Created { get; set; }
        public string CreatedBy { get; set; }
        public uint Difficulty { get; set; }
        public QuestionState State { get; set; }
        public List<string> Tags { get; set; }
    }

    public enum QuestionState
    {
        Pending,
        Published,
        Archived
    }
}
