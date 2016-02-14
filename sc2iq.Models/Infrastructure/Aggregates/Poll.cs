using sc2iq.Models.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Aggregates
{
    public class Poll : EventSourced
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public uint Votes { get; private set; }

        protected Poll(Guid id) : base(id)
        {
            base.Handles<PollVoteAddedEvent>(this.OnVoteAdded);
            base.Handles<PollVoteAddedEvent>(this.OnVoteAdded);
        }

        public Poll(string title, string descrption)
            : this(Guid.NewGuid())
        {
            this.Update(new PollCreatedEvent() {
                SourceId = this.Id,
                Title = title,
                Description = descrption
            });
        }

        public void VoteAdd()
        {
            this.Update(new PollVoteAddedEvent() { SourceId = this.Id });
        }

        public void VoteRemove()
        {
            if(Votes == 0)
            {
                throw new InvalidOperationException("Cannot remove vote from poll because votes is already at 0.");
            }

            this.Update(new PollVoteRemovedEvent() { SourceId = this.Id });
        }

        private void OnVoteAdded(PollVoteAddedEvent e)
        {
            this.Votes++;
        }

        private void OnVoteRemoved(PollVoteRemovedEvent e)
        {
            this.Votes--;
        }
    }
}
