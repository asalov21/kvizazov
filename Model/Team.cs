using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvizazov.Model
{
    public class Team
    {
        public string Name { get; set; }
        public User Captain { get; set; }
        public List<User> Members { get; set; }
        public TeamType Type { get; set; }
        public TeamOccupancy Occupancy { get; set; }
        public TeamVisibility Visibility { get; set; }
        public string AccessCode { get; set; }
        public List<int> SignedUpQuizzes { get; set; }
        public string ConcatenatedUsernames
        {
            get 
            { 
                try
                {
                    return Members.Select(user => user.Username).Aggregate((current, next) => current + "," + next);
                }catch(Exception e)
                {
                    return "";
                }
                
            }
        }
    }
}
