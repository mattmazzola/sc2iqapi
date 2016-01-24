using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Aggregates
{
    public class Poll
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public uint Votes { get; private set; }

        public Poll(string title, string description)
        {
            Id = Guid.NewGuid();
            Votes = 0;
        }

        public void AddVote()
        {
            Votes++;
        }

        public void RemoveVote()
        {
            if(Votes == 0)
            {
                throw new InvalidOperationException("Cannot remove vote from poll because votes is already at 0.");
            }

            Votes--;
        }
    }
}
