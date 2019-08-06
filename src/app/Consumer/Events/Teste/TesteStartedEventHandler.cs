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
        private readonly DI _teste;
        public TesteStartedEventHandler(DI teste)
        {
            _teste = teste;
        }
        public Task Handle(TesteStarted @event)
        {
            // Executar algo
            var ae = _teste.Nome;
            return Task.CompletedTask;
        }
    }
}
