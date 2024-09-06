using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvizazov.Model
{
    public class Quiz
    {
        public int Id { get; set; }
        public QuizType Type { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int NumQuestions { get; set; }
        public int SecondsPerQuestion { get; set; }
        public Dictionary<Team,int> LeaderboardPairTeam { get; set; }
        public Dictionary<User,int> LeaderboardSolo { get; set; }
        public QuizStatus Status { get; set; }
    }
}
