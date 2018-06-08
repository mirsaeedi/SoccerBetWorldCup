using SoccerBet.Data.Models;
using SoccerBet.Data.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.ScoreEngine
{
    public class ScoreCalculator
    {
        private readonly BetGroupMatchPredictionRule[] _matchRules;
        private readonly BetGroupBonusPredictionRule[] _bonusRules;

        public ScoreCalculator(BetGroupMatchPredictionRule[] matchRules, BetGroupBonusPredictionRule[] bonusRules)
        {
            _matchRules = matchRules;
            _bonusRules = bonusRules;
        }
        public MatchPredictionResult[] CalculateScoreForUser(User user, Match[] matches, MatchPrediction[] matchPredictions,
            BonusPrediction[] bonusPredictions)
        {
            var result = new List<MatchPredictionResult>();

            var matchPredictionResult = CalculateMatchesScore(user, matches,matchPredictions);
            result.AddRange(matchPredictionResult);
            //var bonusScore = CalculateBonusScore(user, matches, bonusPredictions);

            return result.ToArray();
        }

        private double CalculateBonusScore(User user, Match[] matches, BonusPrediction[] bonusPredictions)
        {
            throw new NotImplementedException();
        }

        private MatchPredictionResult[] CalculateMatchesScore(User user, Match[] matches, MatchPrediction[] matchPredictions)
        {
            var result = new List<MatchPredictionResult>();

            foreach (var match in matches)
            {
                var matchPredictionResult = new MatchPredictionResult()
                {
                    Match = match
                };

                if (match.DateTime < DateTime.Now)
                {
                    var matchPrediction = matchPredictions.SingleOrDefault(q => q.MatchId == match.Id);
                    matchPredictionResult.MatchPrediction = matchPrediction;

                    var v = CalculateMatchScore(match, matchPrediction);
                    matchPredictionResult.MatchPredictionType = v.MatchPredictionType;
                    matchPredictionResult.PenaltyPredictionType = v.PenaltyPredictionType;
                    matchPredictionResult.Score = v.score;
                }
                else
                {
                    matchPredictionResult.MatchPredictionType = null;
                    matchPredictionResult.Score = 0;
                    matchPredictionResult.MatchPrediction = null;
                }

                result.Add(matchPredictionResult);
            }

            return result.ToArray();
        }

        public (MatchPredictionType? MatchPredictionType, 
            PenaltyPredictionType? PenaltyPredictionType, 
            double score)CalculateMatchScore(Match match, MatchPrediction matchPrediction)
        {
            if (!match.MatchHasStarted)
                return (null,null,0);

            if (matchPrediction == null)
                return (null, null, 0);

            if (matchPrediction.HomeTeamScore.MatchResult is null
                || matchPrediction.AwayTeamScore.MatchResult is null)
                return (null, null, 0);

            var matchRule = GetPredictionRule(match);
            var matchPredictionType = GetMatchPredictionType(match, matchPrediction);
            var penaltyPredictionType = GetPenaltyPredictionType(match, matchPrediction);
            var score = 0.0;

            if (matchRule.UseFormulaForComputingScore)
            {
                score = matchRule.GetScoreBasedOnFormula(match, matchPrediction, penaltyPredictionType);
            }
            else
            {
                score = matchRule.GetScoreOfPredictionType(matchPredictionType, penaltyPredictionType);
            }

            return (matchPredictionType, penaltyPredictionType,score);
            
        }

        private PenaltyPredictionType GetPenaltyPredictionType(Match match, MatchPrediction matchPrediction)
        {
            PenaltyPredictionType predictionType;

            if (match.PenaltyWinner != null && matchPrediction.PenaltyWinner != null
                && match.PenaltyWinner.Id == matchPrediction.PenaltyWinner.Id)
                predictionType = PenaltyPredictionType.Correct;
            else
                predictionType = PenaltyPredictionType.Wrong;

            return predictionType;
        }

        private MatchPredictionType GetMatchPredictionType(Match match, MatchPrediction matchPrediction)
        {
            MatchPredictionType predictionType;

            if (match.HomeTeamScore.MatchResult.Value == matchPrediction.HomeTeamScore.MatchResult.Value)
                predictionType = MatchPredictionType.Exact;
            else if (match.GoalDifference == matchPrediction.GoalDifference)
                predictionType= MatchPredictionType.GoalDifference;
            else if (match.MatchWinner != null && matchPrediction.MatchWinner != null
                && match.MatchWinner.Id == matchPrediction.MatchWinner.Id)
                predictionType= MatchPredictionType.MatchWinner;
            else
                predictionType=  MatchPredictionType.Wrong;

            return predictionType;
        }

        private BetGroupMatchPredictionRule GetPredictionRule(Match match)
        {
            var matchRule = _matchRules.SingleOrDefault(q=>q.MatchId==match.Id);

            if (matchRule != null)
                return matchRule;

            matchRule = _matchRules
                .SingleOrDefault(q => q.MatchId is null && q.MatchType == match.MatchType);

            if (matchRule != null)
                return matchRule;

            return _matchRules
                .SingleOrDefault(q => !q.MatchId.HasValue && !q.MatchType.HasValue);
        }
    }
}
