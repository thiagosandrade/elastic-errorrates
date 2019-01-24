using System.Threading.Tasks;

namespace ElasticErrorRates.Core.SignalR
{
    public interface ISignalRHub
    {
        Task BroadCastMessage(SignalRMessage message);
    }
}