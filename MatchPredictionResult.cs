using SoccerBet.Data.Models;
using SoccerBet.Data.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet
{
    public class MatchPredictionResult
    {
        public Match Match { get; set; }
        public MatchPrediction MatchPrediction { get; set; }
        public MatchPredictionType? MatchPredictionType { get; set; }
        public double Score { get; set; }
        public PenaltyPredictionType? PenaltyPredictionType { get; internal set; }
    }
}
