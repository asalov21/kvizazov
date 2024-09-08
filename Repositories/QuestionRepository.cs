using Kvizazov.Forms;
using Kvizazov.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvizazov.Repositories
{
    public class QuestionRepository
    {
        private readonly HttpRequestService requestService = new HttpRequestService();

        public async Task CreateQuestion(Question question)
        {
            string json = JsonConvert.SerializeObject(question);
            await requestService.HttpPatchRequest($"questions/{question.Id}", json);
        }

        public async Task CreatePotentialQuestion(Question question)
        {
            string json = JsonConvert.SerializeObject(question);
            await requestService.HttpPatchRequest($"potentialQuestions/{question.Id}", json);
        }

        public async Task<int> GetNextPotentialQuestionId()
        {
            string responseJson = await requestService.HttpGetRequestOnlyLastElement("potentialQuestions","id");
            Dictionary<string,Question> responseQuestion = JsonConvert.DeserializeObject<Dictionary<string,Question>>(responseJson);
            return responseQuestion.First().Value.Id + 1;
        }

        public async Task<int> GetNextQuestionId()
        {
            string responseJson = await requestService.HttpGetRequestOnlyLastElement("questions", "id");
            Dictionary<string, Question> responseQuestion = JsonConvert.DeserializeObject<Dictionary<string, Question>>(responseJson);
            return responseQuestion.First().Value.Id + 1;
        }

        public async Task<List<Question>> GetAllPotentialQuestions()
        {
            string responseJson = await requestService.HttpGetRequest("potentialQuestions");
            try 
            {
                return JsonConvert.DeserializeObject<List<Question>>(responseJson);
            }catch(Exception e)
            {
                List<Question> potentialQuestions = new List<Question>();
                Dictionary<string,Question> responseQuestions = JsonConvert.DeserializeObject<Dictionary<string,Question>>(responseJson);
                foreach(Question question in responseQuestions.Values)
                {
                    potentialQuestions.Add(question);
                }
                return potentialQuestions;
            }
            
        }

        public async Task<int> GetAllNonNullQuestionsNumber()
        {
            string responseJson = await requestService.HttpGetRequest("questions");
            responseJson = string.Join(",", responseJson.TrimEnd(']').Split(',').Skip(1));
            responseJson = $"[{responseJson}]";
            List<Question> allQuestions = JsonConvert.DeserializeObject<List<Question>>(responseJson);
            return allQuestions.Where(question => question != null).ToList().Count;
        }

        public async Task<List<Question>> GetRandomQuestions(int numberOfQuestions)
        {
            string responseJson = await requestService.HttpGetRequest("questions");
            responseJson = string.Join(",", responseJson.TrimEnd(']').Split(',').Skip(1));
            responseJson = $"[{responseJson}]";
            List<Question> allQuestions = JsonConvert.DeserializeObject<List<Question>>(responseJson);
            List<Question> randomQuestions = new List<Question>();
            Random random = new Random();
            for (int i = 0; i < numberOfQuestions; i++)
            {
                int randomIndex;
                do
                {
                    randomIndex = random.Next(allQuestions.Count);
                } while (allQuestions[randomIndex] == null);

                randomQuestions.Add(allQuestions[randomIndex]);
                allQuestions.RemoveAt(randomIndex);
            }
            return randomQuestions;
        }

        public async Task DeletePotentialQuestion(Question question)
        {
            await requestService.HttpDeleteRequest($"potentialQuestions/{question.Id}");
        }
    }
}
