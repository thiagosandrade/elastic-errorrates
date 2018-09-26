using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticErrorRates.API.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ElasticErrorRates.API.Controllers
{
    [Route("api/SignalR")]
    public class SignalRController : Controller
    {
        private readonly IHubContext<NotifyHub, ITypedHubClient> _hubContext;

        public SignalRController(IHubContext<NotifyHub, ITypedHubClient> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public string Post([FromBody] SignalRMessage message)
        {
            string retMessage;
            try
            {
                _hubContext.Clients.All.BroadCastMessage(message);
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