using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.ViewModels.QueryResult
{
    public class UserRankQueryResult
    {
        public string UserName { get; internal set; }
        public string UserImageUrl { get; internal set; }
        public double MatchScore { get; internal set; }
        public int BonusScore { get; internal set; }
        public int CorrectPredictions { get; internal set; }
        public int GoalDifferencePredictions { get; internal set; }
        public int MatchWinnerPredictions { get; internal set; }
        public int WrongPredictions { get; internal set; }
        public long UserId { get; internal set; }
    }
}
