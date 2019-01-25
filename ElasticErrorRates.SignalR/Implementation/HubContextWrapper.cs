using System.Threading.Tasks;
using ElasticErrorRates.Core.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace ElasticErrorRates.SignalR.Implementation
{
    public class HubContextWrapper : IHubContextWrapper
    {
        private readonly IHubContext<SignalRHub> _hubContext;

        public HubContextWrapper(IHubContext<SignalRHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public IHubContext<SignalRHub> GetContext()
        {
            return _hubContext;
        }

        public async Task SendMessage(SignalRMessage message)
        {
            await _hubContext.Clients.All.SendAsync("BroadcastMessage", new object[]{ message } );
        }
    }
}
