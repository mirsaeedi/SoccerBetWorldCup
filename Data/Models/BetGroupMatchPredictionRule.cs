using Microsoft.AspNetCore.Identity;
using SoccerBet.Data.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.Data.Models
{
    public class BetGroupMatchPredictionRule : Entity
    {
        public long BetGroupId { get; set; }
        public BetGroup BetGroup { get; set; }
        public short ExactMatchResultPredictionScore { get; set; }
        public short GoalDifferencePredictionScore { get; set; }
        public short WinnerPredictionScore { get; set; }
        public short WrongPredictionScore { get; set; }
        public short PenaltyPredictionScore { get; set; }
        public short PredictionScore { get; set; }
        public MatchType? MatchType { get; set; }
        public long? MatchId { get; set; }
        public Match Match { get; set; }
        public double ScoreCoefficient { get; set; }
        public bool UseFormulaForComputingScore { get; set; }
        internal double GetScoreOfPredictionType(MatchPredictionType matchPredictionType,PenaltyPredictionType penaltyPredictionType)
        {
            var rawScore = 0;

            if (matchPredictionType == MatchPredictionType.Exact)
                rawScore = ExactMatchResultPredictionScore;

            if (matchPredictionType == MatchPredictionType.GoalDifference)
                rawScore = GoalDifferencePredictionScore;

            if (matchPredictionType == MatchPredictionType.MatchWinner)
                rawScore = WinnerPredictionScore;

            if (matchPredictionType == MatchPredictionType.Wrong)
                rawScore = WrongPredictionScore;

            if (penaltyPredictionType == PenaltyPredictionType.Correct)
                rawScore += PenaltyPredictionScore;

            return rawScore * ScoreCoefficient;
        }
        internal double GetScoreBasedOnFormula(Match match, MatchPrediction matchPrediction, PenaltyPredictionType penaltyPredictionType)
        {
            var rawScore = (short)Math.Max(2, 10 - 2 *
                   Math.Abs(
                       Math.Abs(match.HomeTeamScore.MatchResult.Value - match.AwayTeamScore.MatchResult.Value)
                       -
                       Math.Abs(matchPrediction.HomeTeamScore.MatchResult.Value - matchPrediction.AwayTeamScore.MatchResult.Value))
                       -
                       ((Math.Abs(match.AwayTeamScore.MatchResult.Value - matchPrediction.AwayTeamScore.MatchResult.Value))
                       +
                       (Math.Abs(match.HomeTeamScore.MatchResult.Value - matchPrediction.HomeTeamScore.MatchResult.Value))));

            if (penaltyPredictionType == PenaltyPredictionType.Correct)
                rawScore += PenaltyPredictionScore;

            return rawScore * ScoreCoefficient;
        }

    }
}
