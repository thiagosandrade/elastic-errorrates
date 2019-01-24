using ElasticErrorRates.Core.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace ElasticErrorRates.SignalR.Implementation
{
    public class SignalRHub : Hub<ISignalRHub>
    {
    }
}
