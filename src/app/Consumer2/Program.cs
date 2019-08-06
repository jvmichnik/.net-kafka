using Confluent.Kafka;
using Consumer.Events.Teste;
using EventBus;
using EventBus.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var sp = new ServiceCollection()
                .AddSingleton<IEventBusSubscriptionsManager, SubscriptionsManager>()
                .BuildServiceProvider();

            var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

            var bus = new Bus("localhost:9092", eventBusSubcriptionsManager,sp);

            bus.Subscribe<TesteStarted, TesteStartedEventHandler>("testtopic");

            bus.StartConsume("test-consumer-group3");

        }
    }
    
}
