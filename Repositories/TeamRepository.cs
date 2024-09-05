using Kvizazov.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kvizazov.Repositories
{
    public class TeamRepository
    {
        private readonly HttpRequestService requestService = new HttpRequestService();

        public async Task CreateNewTeam(Team team)
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

        /*
        public async Task<List<Team>> ShowFilteredTeams(TeamType type, TeamOccupancy occupancy, TeamVisibility visibility)
        {
            List<Team> allTeams = await GetAllTeams();
            List<Team> teamsToShow = new List<Team>();

            foreach(Team team in allTeams)
            {
                if(team.Type == type && team.Occupancy == occupancy && team.Visibility == visibility)
                {
                    teamsToShow.Add(team);
                }
            }

            return teamsToShow;
        }
        */
    }
}
