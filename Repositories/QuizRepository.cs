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

        public async Task<Quiz> GetQuizById(int id)
        {
            string response = await requestService.HttpGetRequest($"quizzes/{id}");
            return JsonConvert.DeserializeObject<Quiz>(response);
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

        public async Task<List<Quiz>> UpdateQuizStatus(List<Quiz> quizzesBeforeUpdate)
        {
            List<Quiz> quizzesAfterUpdate = new List<Quiz>();

            foreach (Quiz quiz in quizzesBeforeUpdate)
            {
                if (quiz.Status == QuizStatus.Otvoren && DateTime.Now > quiz.End)
                {
                    quiz.Status = QuizStatus.Zatvoren;
                    await CreateOrUpdateQuiz(quiz);
                }
                if(quiz.Status == QuizStatus.Zatvoren && DateTime.Now > quiz.Start && DateTime.Now < quiz.End)
                {
                    quiz.Status = QuizStatus.Otvoren;
                    await CreateOrUpdateQuiz(quiz);
                }
                quizzesAfterUpdate.Add(quiz);
            }

            return quizzesAfterUpdate;
        }

        public async Task<List<Quiz>> GetAllOpenQuizzesUserSignedUp(User user)
        {
            List<int> quizzesUserSignedUp = user.SignedUpQuizzes;

            List<Quiz> allQuizzesBeforeUpdate = new List<Quiz>();

            foreach(int quizId in quizzesUserSignedUp)
            {
                Quiz quiz = await GetQuizById(quizId);
                allQuizzesBeforeUpdate.Add(quiz);
            }

            if(allQuizzesBeforeUpdate.Where(_quiz => _quiz != null).ToList().Count == 0)
            {
                return new List<Quiz>();
            }

            List<Quiz> allQuizzesAfterUpdate = await UpdateQuizStatus(allQuizzesBeforeUpdate.Where(_quiz => _quiz != null).ToList());

            return allQuizzesAfterUpdate.Where(quiz => quiz.Status == QuizStatus.Otvoren).ToList();
        }

        public async Task<List<Quiz>> GetAllOpenQuizzesTeamSignedUp(string teamName)
        {
            Team team = await new TeamRepository().GetTeamByName(teamName);

            List<int> quizzesTeamSignedUp = team.SignedUpQuizzes;

            List<Quiz> allQuizzesBeforeUpdate = new List<Quiz>();

            foreach(int quizId in quizzesTeamSignedUp)
            {
                Quiz quiz = await GetQuizById(quizId);
                allQuizzesBeforeUpdate.Add(quiz);
            }

            if(allQuizzesBeforeUpdate.Where(_quiz => _quiz != null).ToList().Count == 0)
            {
                return new List<Quiz>();
            }
            List<Quiz> allQuizzesAfterUpdate = await UpdateQuizStatus(allQuizzesBeforeUpdate.Where(_quiz => _quiz != null).ToList());

            return allQuizzesAfterUpdate.Where(quiz => quiz.Status == QuizStatus.Otvoren).ToList();
        }
    }
}