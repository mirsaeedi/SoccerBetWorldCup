using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.Data.Models.Constants
{
    public enum MatchPredictionType
    {
        Exact=0,
        GoalDifference=1,
        MatchWinner = 2,
        HasPenalty = 3,
        Wrong = 4,
        
    }
}

