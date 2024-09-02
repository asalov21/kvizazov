using Kvizazov.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvizazov.Services
{
    public class JsonParserService
    {
        public string UserToJson_Registration(User user)
        {
            return $"{{\"{user.Username}\":{{\"email\":\"{user.Email}\",\"name\":\"{user.Name}\",\"password\":\"{user.Password}\",\"surname\":\"{user.Surname}\",\"username\":\"{user.Username}\",\"soloPoints\":{user.SoloPoints},\"pairPoints\":{user.PairPoints},\"teamPoints\":{user.TeamPoints}}}}}";
        }

        public List<User> JsonToUserList(string json)
        {
            List<User> userList = new List<User>();
            json = json.TrimStart('{').TrimEnd('}');
            string[] userStrings = json.Split(',');

            foreach (string userString in userStrings)
            {
                string[] keyValuePairs = userString.Split(':');
                string username = keyValuePairs[0].Trim('\"');
                string[] userProperties = keyValuePairs[1].TrimStart('{').TrimEnd('}').Split(',');
                User user = new User();
                user.Username = username;

                foreach (string property in userProperties)
                {
                    string[] propertyKeyValuePair = property.Split(':');
                    string propertyName = propertyKeyValuePair[0].Trim('\"');
                    string propertyValue = propertyKeyValuePair[1].Trim('\"');

                    switch (propertyName)
                    {
                        case "email":
                            user.Email = propertyValue;
                            break;
                        case "name":
                            user.Name = propertyValue;
                            break;
                        case "password":
                            user.Password = propertyValue;
                            break;
                        case "surname":
                            user.Surname = propertyValue;
                            break;
                        case "soloPoints":
                            user.SoloPoints = int.Parse(propertyValue);
                            break;
                        case "pairPoints":
                            user.PairPoints = int.Parse(propertyValue);
                            break;
                        case "teamPoints":
                            user.TeamPoints = int.Parse(propertyValue);
                            break;
                    }
                }

                userList.Add(user);
            }

            return userList;
        }

        public User JsonToUser(string json)
        {
            json = json.TrimStart('{').TrimEnd('}');
            string[] keyValuePairs = json.Split(',');

            User user = new User();

            foreach (string property in keyValuePairs)
            {
                string[] propertyKeyValuePair = property.Split(':');
                string propertyName = propertyKeyValuePair[0].Trim('\"');
                string propertyValue = propertyKeyValuePair[1].Trim('\"');

                switch (propertyName)
                {
                    case "email":
                        user.Email = propertyValue;
                        break;
                    case "name":
                        user.Name = propertyValue;
                        break;
                    case "password":
                        user.Password = propertyValue;
                        break;
                    case "surname":
                        user.Surname = propertyValue;
                        break;
                    case "soloPoints":
                        user.SoloPoints = int.Parse(propertyValue);
                        break;
                    case "pairPoints":
                        user.PairPoints = int.Parse(propertyValue);
                        break;
                    case "teamPoints":
                        user.TeamPoints = int.Parse(propertyValue);
                        break;
                }
            }

            return user;
        }
    }
}
