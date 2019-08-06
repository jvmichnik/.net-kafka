using EventBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer.Events.Teste
{
    public class TesteStartedEventHandler :
        IIntegrationEventHandler<TesteStarted>
    {
        public Task Handle(TesteStarted @event)
        {
            // Executar algo

            return Task.CompletedTask;
        }
    }
}
