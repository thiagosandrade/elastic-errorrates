using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;

namespace ElasticErrorRates.API.SignalR
{
    public class NotifyHub : Hub<ITypedHubClient>
    {
    }
}
