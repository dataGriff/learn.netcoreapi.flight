using System;
using System.Threading.Tasks;

namespace FlightPlanApi.Authentication
{
    public class UserService : IUserService
    {

        // super simple auth for this example
        // in a real app, this would be a database call
        // this simply only allows admin and the password below to interact with the API
        public Task<User> Authenticate(string username, string password)
        {
            if (username != "admin" || password != "P@ssw0rd")
            {
                return Task.FromResult<User>(null);
            }

        // this will create a new user with a GUID as the ID
        // the username will always be admin at this point but will get diff ids
            var user = new User
            {
                Username = username,
                Id = Guid.NewGuid().ToString("N")
            };

            return Task.FromResult(user);
        }
    }
}