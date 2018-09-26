using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticErrorRates.API.SignalR
{
    public class SignalRMessage
    {
        public string Type { get; set; }
        public string Payload { get; set; }
    }
}
