namespace ElasticErrorRates.Core.SignalR
{
    public interface ISignalRHub
    {
        Task BroadCastMessage(SignalRMessage message);
    }
}