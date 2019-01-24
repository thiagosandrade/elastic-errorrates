using System;
using ElasticErrorRates.Core.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace ElasticErrorRates.API.Controllers
{
    [Route("api/SignalR")]
    public class SignalRController : Controller
    {
        private readonly ISignalRHub _hubContext;

        public SignalRController(ISignalRHub hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public string Post([FromBody] SignalRMessage message)
        {
            string retMessage;
            try
            {
                _hubContext.BroadCastMessage(message);
                //_hubContext.Clients.All.BroadCastMessage(message);
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }

            return retMessage;
        }
    }
}