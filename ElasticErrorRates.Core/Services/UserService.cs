using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ElasticErrorRates.API.Services;
using ElasticErrorRates.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ElasticErrorRates.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IOptions<AppSettings> _appSettings;

        private readonly List<User> _users = new List<User>
        { 
            new User { Id = 1, FirstName = "Test", LastName = "User", Username = "TestUserName", Password = "123" } 
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
                return null;

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
    }
}
