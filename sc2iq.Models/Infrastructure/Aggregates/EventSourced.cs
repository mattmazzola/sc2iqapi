using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sc2iq.Models.Infrastructure.Aggregates
{
    public abstract class EventSourced : IEventSourced
    {
        private readonly Dictionary<Type, Action<IVersionedEvent>> handlers = new Dictionary<Type, Action<IVersionedEvent>>();
        private readonly List<IVersionedEvent> pendingEvents = new List<IVersionedEvent>();

        private readonly Guid id;
        private int version = -1;

        protected EventSourced(Guid id)
        {
            this.id = id;
        }

        public Guid Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets the entity's version. As the entity is being updated and events being generated, the version is incremented.
        /// </summary>
        public int Version
        {
            get { return this.version; }
            protected set { this.version = value; }
        }
        
        public IEnumerable<IVersionedEvent> Events
        {
            get { return this.pendingEvents; }
        }

        protected void Handles<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        {
            this.handlers.Add(typeof(TEvent), @event => handler((TEvent)@event));
        }

        protected void LoadFrom(IEnumerable<IVersionedEvent> pastEvents)
        {
            foreach (var e in pastEvents)
            {
                this.handlers[e.GetType()].Invoke(e);
                this.version = e.Version;
            }
        }

        protected void Update(VersionedEvent e)
        {
            e.SourceId = this.Id;
            e.Version = this.version + 1;
            this.handlers[e.GetType()].Invoke(e);
            this.version = e.Version;
            this.pendingEvents.Add(e);
        }
    }
}
