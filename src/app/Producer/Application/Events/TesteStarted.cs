using EventBus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Producer.Application.Events
{
    public class TesteStarted : IntegrationEvent
    {
        public TesteStarted(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
