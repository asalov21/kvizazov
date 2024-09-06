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

        public async Task DeletePotentialQuestion(Question question)
        {
            await requestService.HttpDeleteRequest($"potentialQuestions/{question.Id}");
        }
    }
}
