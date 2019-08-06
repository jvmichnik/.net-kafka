using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Producer.Application;
using Producer.Application.Events;

namespace Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesteController : ControllerBase
    { 

        private readonly IProducerEventService _producerEventService;
        public TesteController(IProducerEventService producerEventService)
        {
            _producerEventService = producerEventService ?? throw new ArgumentNullException(nameof(producerEventService));
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(string name)
        {
            var teste = new TesteStarted(name);
            _producerEventService.PublishEventAsync(teste, "testtopic");
            return Ok("foi");
        }
    }
}
