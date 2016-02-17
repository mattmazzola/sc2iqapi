using sc2iq.Models.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Aggregates
{
    public class Question : EventSourced
    {
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

        protected Question(Guid id) : base(id)
        {
            base.Handles<QuestionCreatedEvent>(this.OnQuestionCreated);
            base.Handles<QuestionPublishedEvent>(this.OnQuestionPublished);
        }

        public Question(
            string q,
            string a1,
            string a2,
            string a3,
            string a4,
            int correctAnswerIndex,
            DateTimeOffset created,
            string createdBy,
            uint difficulty,
            QuestionState state,
            List<string> tags
            )
            : this(Guid.NewGuid())
        {
            this.Update(new QuestionCreatedEvent()
            {
                SourceId = this.Id,
                Q = q,
                A1 = a1,
                A2 = a2,
                A3 = a3,
                A4 = a4,
                CorrectAnswerIndex = correctAnswerIndex,
                Created = created,
                CreatedBy = createdBy,
                Difficulty = difficulty,
                State = state,
                Tags = tags
            });
        }

        public void Publish()
        {
            this.Update(new QuestionPublishedEvent() { SourceId = this.Id });
        }

        private void OnQuestionCreated(QuestionCreatedEvent e)
        {
            // empty
        }

        private void OnVoteAdded(PollVoteAddedEvent e)
        {
            this.State = QuestionState.Published;
        }
    }

    public enum QuestionState
    {
        Pending,
        Published,
        Archived
    }
}
