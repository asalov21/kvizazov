using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvizazov.Model
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> WrongAnswers { get; set; }
        public string ConcatenatedWrongAnswers
        {
            get
            {
                try
                {
                    return WrongAnswers.Aggregate((current, next) => current + "," + next);
                }
                catch (Exception e)
                {
                    return "";
                }

            }
        }

    }
}
