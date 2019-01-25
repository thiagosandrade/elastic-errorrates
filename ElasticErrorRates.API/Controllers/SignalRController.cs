using System;
using System.Threading.Tasks;
using ElasticErrorRates.Core.CQRS.Command;
using ElasticErrorRates.Core.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace ElasticErrorRates.API.Controllers
{
    [Route("api/SignalR")]
    public class SignalRController : Controller
    {
        private readonly IHubContextWrapper _hubContext;
        private readonly ICommandDispatcher _commandDispatcher;

        public SignalRController(IHubContextWrapper hubContext, ICommandDispatcher commandDispatcher)
        {
            _hubContext = hubContext;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SignalRMessage message)
        {
            try
            {
                await _commandDispatcher.DispatchAsync(_hubContext.SendMessage, message);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}