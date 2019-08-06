using EventBus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consumer.Events.Teste
{
    public class TesteStarted : IntegrationEvent
    {
        public TesteStarted(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; private set; }
    }
}
