using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoccerBet.Data;
using SoccerBet.Data.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SoccerBet
{
    public class LiveScoreUpdaterJob
    {
        private IHttpClientFactory _httpClientFactory;
        private SoccerBetDbContext _dbContext;
        private readonly string _liveUrl = "/v1/competitions/467/fixtures";

        public LiveScoreUpdaterJob(SoccerBetDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
        }

        public async Task Run()
        {
            var currentMatches = await _dbContext
                .Matches
                .Include(m=>m.HomeTeamScore.Team)
                .Include(m => m.AwayTeamScore.Team)
                .Where(q => q.DateTime.LocalDateTime < DateTime.Now && q.MatchStatus!=MatchStatus.Finished)
                .ToArrayAsync();


            if (currentMatches.Length == 0)
                return;

            var httpClient =  _httpClientFactory.CreateClient("LiveScoreAPIClient");
            var httpResponseMessage = await httpClient.GetAsync(_liveUrl);


            var jsonResult = await httpResponseMessage.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(jsonResult,
                new JsonSerializerSettings() { DateParseHandling = DateParseHandling.None });

            JArray fixtures = result.fixtures;
            
            foreach (var match in currentMatches)
            {
                var homeTeamName = match.HomeTeamScore.Team.Name;
                var awayTeamName = match.AwayTeamScore.Team.Name;
                var matchDate = match.DateTime.LocalDateTime;

                for (int i = 0; i < fixtures.Count; i++)
                {
                    dynamic fixture = fixtures[i];
                    var date = DateTime.Parse((string)fixture.date);

                    if (homeTeamName == (string)fixture.homeTeamName
                        && awayTeamName == (string)fixture.awayTeamName
                        && matchDate==date)
                    {
                        string status = fixture.status;

                        if (status == "IN_PLAY")
                        {
                            match.MatchStatus = MatchStatus.InPlay;
                        }
                        else if(status == "FINISHED")
                        {
                            match.MatchStatus = MatchStatus.Finished;
                        }

                        match.HomeTeamScore.PenaltyResult = (short?)fixture.result.penaltyShootout?.goalsHomeTeam;
                        match.AwayTeamScore.PenaltyResult = (short?)fixture.result.penaltyShootout?.goalsAwayTeam;

                        match.HomeTeamScore.MatchResult = (short?)fixture.result.goalsHomeTeam;
                        match.AwayTeamScore.MatchResult = (short?)fixture.result.goalsAwayTeam;

                        try
                        {
                            match.HomeTeamScore.MatchResult = (short?)fixture.result.extraTime.goalsHomeTeam;
                            match.AwayTeamScore.MatchResult = (short?)fixture.result.extraTime.goalsAwayTeam;
                        }
                        catch (Exception)
                        {
                        }
                        

                    }
                }
            }

            await _dbContext.SaveChangesAsync();

        }
    }
}
