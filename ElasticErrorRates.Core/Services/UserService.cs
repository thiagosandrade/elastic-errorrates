using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ElasticErrorRates.API.Services;
using ElasticErrorRates.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nest;

namespace ElasticErrorRates.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IOptions<AppSettings> _appSettings;

        private readonly List<User> _users = new()
        { 
            new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "123" } 
        };

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }
        public User Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username.Equals(username)
                                                   && x.Password.Equals(password));

            if (user == null)
                return default!;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;
        }

        public void Register(User userParam)
        {
            _users.Add(userParam);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User? GetById(int id)
        {
            return _users.Where(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public void Update(int id, User user)
        {
            var userDb = _users.Where(x => x.Id.Equals(id)).FirstOrDefault();

            if(userDb != null) 
            {
                userDb.FirstName = user.FirstName;
                userDb.LastName = user.LastName;
                userDb.Username = user.Username;
                userDb.Password = user.Password;
                
                _users.Remove(userDb);
                _users.Add(user);
            }
        }

        public void Delete(int id)
        {
            var userDb = GetById(id);
            if(userDb != null)
            {
                _users.Remove(userDb);
            }
        }
    }
}
