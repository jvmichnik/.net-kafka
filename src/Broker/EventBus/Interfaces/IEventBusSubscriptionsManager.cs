using EventBus.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static EventBus.SubscriptionsManager;

namespace EventBus.Interfaces
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        void Clear();

        IEnumerable<string> GetTopics();

        void AddDynamicSubscription<TH>(string eventName, string topic)
           where TH : IDynamicIntegrationEventHandler;

        void AddSubscription<T, TH>(string topic)
           where T : IntegrationEvent
           where TH : IIntegrationEventHandler<T>;

        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;
        bool HasSubscriptionsForEvent(string eventName);

        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        Type GetEventTypeByName(string eventName);
        string GetEventKey<T>();
    }
}
