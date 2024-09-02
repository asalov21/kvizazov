using Kvizazov.Forms;
using Kvizazov.Model;
using Kvizazov.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Kvizazov.Repositories
{
    public class UserRepository
    {
        private readonly HttpRequestService requestService = new HttpRequestService();
        private readonly JsonParserService jsonParserService = new JsonParserService();

        public async Task RegisterUser(User user)
        {
            string json = jsonParserService.UserToJson_Registration(user);
            await requestService.HttpPatchRequest("users", json);
        }

        public async Task<bool> CheckForExistingUserParameter(string key, string value)
        {
            string response = await requestService.HttpGetRequestWithFilter("users", key, value);
            return response == "{}";
        }

        public async Task<bool> LoginCheckUsernamePassword(string username, string password)
        {
            string response = await requestService.HttpGetRequestWithFilter("users", "\"username\"", $"\"{username}\"");
            User user = jsonParserService.JsonToUser(response);
            return user.Password == password;
        }

        public async Task<bool> CheckIfUserExists(string username)
        {
            string response = await requestService.HttpGetRequestWithFilter("users", "\"username\"", $"\"{username}\"");
            return response != "{}";
        }
    }
}
