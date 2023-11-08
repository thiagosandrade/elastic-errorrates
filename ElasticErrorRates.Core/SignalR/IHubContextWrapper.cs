using Microsoft.AspNetCore.SignalR;


namespace ElasticErrorRates.Core.SignalR
{
    public interface IHubContextWrapper
    {
        IHubContext<SignalRHub> GetContext();
        Task SendMessage(SignalRMessage signalRMessage);
    }
}
