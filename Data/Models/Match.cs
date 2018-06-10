using SoccerBet.Data.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.Data.Models
{
    public class Match:Entity
    {
        public MatchType MatchType { get; set; }
        public TeamScore HomeTeamScore { get; set; }
        public TeamScore AwayTeamScore { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public long StadiumId { get; set; }
        public Stadium Stadium { get; set; }
        public bool MatchHasStarted => DateTime.UtcDateTime < DateTimeOffset.Now;
        public int? GoalDifference
        {
            get
            {
                if (!HomeTeamScore.MatchResult.HasValue || !AwayTeamScore.MatchResult.HasValue)
                    return null;

                return 
                    HomeTeamScore.MatchResult.Value - AwayTeamScore.MatchResult.Value;
            }
        }
        public Team MatchWinner
        {
            get
            {
                if (!HomeTeamScore.MatchResult.HasValue || !AwayTeamScore.MatchResult.HasValue)
                    return null;

                if (MatchResultType == Constants.MatchResultType.Withdraw)
                    return null;

                return HomeTeamScore.MatchResult.Value - AwayTeamScore.MatchResult.Value>0
                    ?HomeTeamScore.Team:AwayTeamScore.Team;
            }
        }

        public MatchResultType? MatchResultType
        {
            get
            {
                if (!HomeTeamScore.MatchResult.HasValue || !AwayTeamScore.MatchResult.HasValue)
                    return null;

                return HomeTeamScore.MatchResult.Value - AwayTeamScore.MatchResult.Value == 0
                    ? Constants.MatchResultType.Withdraw: Constants.MatchResultType.WinLose;
            }
        }

        public bool HasPenalty => HomeTeamScore.PenaltyResult.HasValue;

        public Team PenaltyWinner
        {
            get
            {
                if (!HasPenalty)
                    return null;

                return HomeTeamScore.PenaltyResult.Value - AwayTeamScore.PenaltyResult.Value > 0
                    ? HomeTeamScore.Team : AwayTeamScore.Team;
            }
        }
    }
}
