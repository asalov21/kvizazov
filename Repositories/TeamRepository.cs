using Kvizazov.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace Kvizazov.Repositories
{
    public class TeamRepository
    {
        private readonly HttpRequestService requestService = new HttpRequestService();

        public async Task CreateOrUpdateTeam(Team team)
        {
            string json = JsonConvert.SerializeObject(team);
            await requestService.HttpPatchRequest($"teams/{team.Name}", json);
        }

        public async Task<bool> CheckIfTeamExists(string name)
        {
            string response = await requestService.HttpGetRequest($"teams/{name}");
            return response != "null";
        }

        public async Task<List<Team>> GetAllTeams()
        {
            string responseJson = await requestService.HttpGetRequest("teams");
            
            List<Team> allTeams = new List<Team>();

            Dictionary<string, Team> responseTeams = JsonConvert.DeserializeObject<Dictionary<string, Team>>(responseJson);

            foreach (Team team in responseTeams.Values)
            {

                allTeams.Add(team);
            }

            return allTeams;
        }

        public async Task<List<Team>> ShowFilteredTeams(TeamType type, TeamOccupancy occupancy, TeamVisibility visibility)
        {
            List<Team> allTeams = await GetAllTeams();
            return allTeams.Where(team => team.Type == type && team.Occupancy == occupancy && team.Visibility == visibility).ToList();
        }

        public async Task<List<Team>> ShowMyTeams(User user)
        {
            List<Team> allTeams = await GetAllTeams();
            return allTeams.Where(team => team.Members.Select(_user => _user.Username).ToList().Contains(user.Username)).ToList();
        }

        public async Task DeleteTeam(string name)
        {
            await requestService.HttpDeleteRequest($"teams/{name}");
        }
    }
}
