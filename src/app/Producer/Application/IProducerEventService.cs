using EventBus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Producer.Application
{
    public interface IProducerEventService
    {
        void PublishEventAsync(IntegrationEvent evt, string topic = "");
    }
}
