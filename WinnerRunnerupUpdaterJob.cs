using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoccerBet.Data;
using SoccerBet.Data.Models;
using SoccerBet.Data.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SoccerBet
{
    public class WinnerRunnerupUpdaterJob
    {
        private IHttpClientFactory _httpClientFactory;
        private SoccerBetDbContext _dbContext;
        private readonly string _liveUrl = "/v1/competitions/467/leagueTable";

        public WinnerRunnerupUpdaterJob(SoccerBetDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
        }

        public async Task Run()
        {
            var httpClient =  _httpClientFactory.CreateClient("LiveScoreAPIClient");
            var httpResponseMessage = await httpClient.GetAsync(_liveUrl);

            var jsonResult = await httpResponseMessage.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(jsonResult,
                new JsonSerializerSettings() { DateParseHandling = DateParseHandling.None });

            var standings = result.standings as JObject;
            
            foreach (var group in standings.Properties())
            {
                foreach (var team in group.Children())
                {
                    
                    var firstTeamName = team.ElementAt(0)["team"].Value<string>();
                    var secondTeamName = team.ElementAt(1)["team"].Value<string>();

                    var groupName = "Group " + team.ElementAt(0)["group"];

                    var worldcupGroup = _dbContext.WorldCupGroups.Single(q => q.Name == groupName);
                    var firstTeam = _dbContext.Teams.Single(q => q.Name == firstTeamName);
                    var secondTeam = _dbContext.Teams.Single(q => q.Name == secondTeamName);

                    var lastDateOfGroupMatches = _dbContext.Matches.Where(q => q.MatchType == MatchType.Group
                      && q.HomeTeamScore.Team.WorldCupGroupId == worldcupGroup.Id)
                      .Max(q=>q.DateTime);

                    if(DateTime.Now>lastDateOfGroupMatches.ToLocalTime())
                    {
                        worldcupGroup.WinnerId = firstTeam.Id;
                        worldcupGroup.RunnerupId = secondTeam.Id;
                    }
                }
            }

            await _dbContext.SaveChangesAsync();

        }
    }
}
