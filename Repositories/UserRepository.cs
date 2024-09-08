using Kvizazov.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Kvizazov.Repositories
{
    public class UserRepository
    {
        private readonly HttpRequestService requestService = new HttpRequestService();

        public async Task CreateOrUpdateUser(User user)
        {
            string json = JsonConvert.SerializeObject(user);
            await requestService.HttpPatchRequest($"users/{user.Username}", json);
        }

        public async Task<bool> CheckForExistingUserParameter(string key, string value)
        {
            string response = await requestService.HttpGetRequestWithFilter("users", key, value);
            return response == "{}";
        }

        public async Task<bool> CheckIfUserExists(string username)
        {
            string response = await requestService.HttpGetRequest($"users/{username}");
            return response != "null";
        }

        public async Task<User> GetUserByUsername(string username)
        {
            string response = await requestService.HttpGetRequest($"users/{username}");
            return JsonConvert.DeserializeObject<User>(response);
        }

        public async Task<List<User>> GetAllUsers()
        {
            string response = await requestService.HttpGetRequest("users");

            List<User> allUsers = new List<User>();

            Dictionary<string, User> responseUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(response);

            foreach (User user in responseUsers.Values)
            {

                allUsers.Add(user);
            }

            return allUsers;
        }
    }
}
