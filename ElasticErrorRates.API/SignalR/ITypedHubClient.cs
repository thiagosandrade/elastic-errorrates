using System.Threading.Tasks;

namespace ElasticErrorRates.API.SignalR
{
    public interface ITypedHubClient
    {
        Task BroadCastMessage(SignalRMessage message);
    }
}