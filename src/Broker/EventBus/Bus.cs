using EventBus.Interfaces;
using EventBus.Models;
using Newtonsoft.Json;
using System;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace EventBus
{
    public class Bus : IEventBus
    {
        private readonly string KafkaEndpoint;

        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceProvider _serviceProvider;
        public Bus(string kafkaEndpoint, IEventBusSubscriptionsManager subsManager, IServiceProvider serviceProvider)
        {
            KafkaEndpoint = kafkaEndpoint;
            _subsManager = subsManager ?? new SubscriptionsManager();
            _serviceProvider = serviceProvider;
        }

        public async Task StartConsume(string groupId)
        {
            var conf = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = KafkaEndpoint,
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.

                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using (var c = new ConsumerBuilder<string, string>(conf).Build())
            {
                c.Subscribe(_subsManager.GetTopics());

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume();
                            await ProcessEvent(cr.Key, cr.Value);
                            Console.WriteLine($"Consumed message '{cr.Key}' at: '{cr.TopicPartitionOffset}'.");
                            
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }
            }
        }

        public async Task Publish(IntegrationEvent @event, string topic)
        {
            var eventName = @event.GetType().Name;
            var topicName = eventName;
            if (!string.IsNullOrEmpty(topic))
            {
                topicName = topic;
            }
            var _config = new ProducerConfig { BootstrapServers = KafkaEndpoint };

            using (var producer = new ProducerBuilder<string, string>(_config).Build())
            {
                try
                {
                    var message = JsonConvert.SerializeObject(@event);
                    await producer.ProduceAsync(topicName, new Message<string, string> { Key = eventName, Value = message });
                }
                catch (ProduceException<string, string> e)
                {
                    //Erroor
                }
                
            }
        }

        public void Subscribe<T, TH>(string topic)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _subsManager.AddSubscription<T, TH>(topic);
        }

        public void SubscribeDynamic<TH>(string eventName, string topic) where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.AddDynamicSubscription<TH>(eventName, topic);
        }

        private async Task ProcessEvent(string eventName, string message)
        {

            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        var handler = ActivatorUtilities.CreateInstance(_serviceProvider, subscription.HandlerType) as IDynamicIntegrationEventHandler;
                        if (handler == null) continue;
                        dynamic eventData = Newtonsoft.Json.Linq.JObject.Parse(message);
                        await handler.Handle(eventData);
                    }
                    else
                    {
                        var handler = ActivatorUtilities.CreateInstance(_serviceProvider, subscription.HandlerType);
                        if (handler == null) continue;
                        var eventType = _subsManager.GetEventTypeByName(eventName);
                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                }
            }
            else
            {
            }
        }
    }
}
