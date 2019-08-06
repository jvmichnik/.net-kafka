using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Interfaces;
using EventBus.Models;

namespace Producer.Application
{
    public class ProducerEventService : IProducerEventService
    {
        private readonly IEventBus _eventBus;
        public ProducerEventService(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }
        public void PublishEventAsync(IntegrationEvent evt, string topic = "")
        {
            _eventBus.Publish(evt, topic);
        }
    }
}
