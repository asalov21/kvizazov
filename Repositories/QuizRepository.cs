using Kvizazov.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvizazov.Repositories
{
    public class QuizRepository
    {
        private readonly HttpRequestService requestService = new HttpRequestService();

        public async Task CreateOrUpdateQuiz(Quiz quiz)
        {
            string json = JsonConvert.SerializeObject(quiz);
            await requestService.HttpPatchRequest($"quizzes/{quiz.Id}", json);
        }

        public async Task<int> GetNextQuizId()
        {
            string responseJson = await requestService.HttpGetRequestOnlyLastElement("quizzes", "id");
            Dictionary<string, Quiz> responseQuiz = JsonConvert.DeserializeObject<Dictionary<string, Quiz>>(responseJson);
            return int.Parse(responseQuiz.First().Key) + 1;
        }

        public async Task<List<Quiz>> GetAllQuizzesOfType(QuizType type)
        {
            string responseJson = await requestService.HttpGetRequest("quizzes");
            responseJson = string.Join(",", responseJson.TrimEnd(']').Split(',').Skip(1));
            responseJson = $"[{responseJson}]";

            List<Quiz> allQuizzes = new List<Quiz>();

            List<Quiz> responseQuizzes = JsonConvert.DeserializeObject<List<Quiz>>(responseJson);

            foreach (Quiz quiz in responseQuizzes)
            {
                if (quiz.Type == type)
                {
                    allQuizzes.Add(quiz);
                }
            }
            return allQuizzes;
        }
    }
}
