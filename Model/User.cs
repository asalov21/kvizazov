using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvizazov.Model
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int SoloPoints { get; set; }
        public int PairPoints { get; set; }
        public int TeamPoints { get; set; }
        public Role Role { get; set; }
        public List<int> SignedUpQuizzes { get; set; }

    }
}
