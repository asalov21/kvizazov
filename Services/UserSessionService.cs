using Kvizazov.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvizazov.Services
{
    public class UserSessionService
    {
        private static UserSessionService _instance;

        public User LoggedInUser { get; private set; }

        private UserSessionService() { }

        public static UserSessionService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserSessionService();
                }
                return _instance;
            }
        }

        public void Login(User user)
        {
            LoggedInUser = user;
        }

        public void Logout()
        {
            LoggedInUser = null;
        }

        public bool IsUserLoggedIn()
        {
            return LoggedInUser != null;
        }

        public bool IsLoggedInUserAdmin()
        {
            return LoggedInUser.Role == Role.Admin;
        }
    }

}
