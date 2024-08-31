using Kvizazov.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kvizazov.Repositories
{
    public class UserRepository
    {
        private readonly HttpRequestService requestService = new HttpRequestService();

        public async Task GetAllUsers()
        {
            string response = await requestService.HttpGetRequest("users");
            MessageBox.Show(response);
        }
    }
}
