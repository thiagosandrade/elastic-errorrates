using ElasticErrorRates.Core.Models;

namespace ElasticErrorRates.API.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
    }
}