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
            return $"{{\"{user.Username}\":{{\"email\":\"{user.Email}\",\"name\":\"{user.Name}\",\"password\":\"{user.Password}\",\"surname\":\"{user.Surname}\",\"username\":\"{user.Username}\",\"soloPoints\":{user.SoloPoints},\"pairPoints\":{user.PairPoints},\"teamPoints\":{user.TeamPoints},\"role\":\"{user.Role}\"}}}}";
        }

        public string UserToJson_Edit(User user)
        {
            return $"{{\"email\":\"{user.Email}\",\"name\":\"{user.Name}\",\"password\":\"{user.Password}\",\"surname\":\"{user.Surname}\"}}";
        }
        public User JsonToUser(string json)
        {
            json = string.Join(":",json.TrimStart('{').TrimEnd('}').Split(':').Skip(1).ToArray()).TrimStart('{');
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
                    case "role":
                        user.Role = propertyValue == "User" ? Role.User : Role.Admin;
                        break;
                    case "username":
                        user.Username = propertyValue;
                        break;
                }
            }

            return user;
        }
    }
}
