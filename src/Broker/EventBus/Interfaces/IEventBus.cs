using EventBus.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Interfaces
{
    public interface IEventBus
    {
        Task StartConsume(string groupId);
        Task Publish(IntegrationEvent @event, string topic);

        void Subscribe<T, TH>(string topic)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void SubscribeDynamic<TH>(string eventName, string topic)
            where TH : IDynamicIntegrationEventHandler;
    }
}
