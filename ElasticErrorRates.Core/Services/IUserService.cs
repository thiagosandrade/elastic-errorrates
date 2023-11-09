using ElasticErrorRates.Core.Models;

namespace ElasticErrorRates.API.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User? GetById(int id);
        void Register(User userParam);
        void Update(int id, User user);
        void Delete(int id);
    }
}