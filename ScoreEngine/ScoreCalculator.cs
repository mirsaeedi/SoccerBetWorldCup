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
        public MatchPredictionResult[] CalculateTotalMatchPredictionScoreForUser(User user, Match[] matches, MatchPrediction[] matchPredictions)
        {
            var result = new List<MatchPredictionResult>();

            var matchPredictionResult = CalculateMatchesScore(user, matches,matchPredictions);
            result.AddRange(matchPredictionResult);

            return result.ToArray();
        }

        internal double CalculateTotalBonusPredictionScoreForUser(User user, Match[] matches,WorldCupGroup[] worldCupGroups, BonusPrediction[] bonusPredictions)
        {
            var result = new List<MatchPredictionResult>();

            var bonusScore = CalculateBonusScore(user, matches,worldCupGroups, bonusPredictions);

            return bonusScore;
        }

        private double CalculateBonusScore(User user, Match[] matches,WorldCupGroup[] worldcupGroups, BonusPrediction[] bonusPredictions)
        {
            var sum = 0;

            var groupBonusPredictions = bonusPredictions
                .Where(q => q.BonusPredictionType == BonusPredictionType.FirstTeamInGroup
                || q.BonusPredictionType == BonusPredictionType.SecondTeamInGroup);

            var groupByGroup = groupBonusPredictions.GroupBy(q=>q.WorldCupGroup.Id);

            foreach (var group in groupByGroup)
            {
                var worldCupGroup = worldcupGroups.Single(q => q.Id == group.Key);

                sum +=CalculateBonusScoreForGroup(
                    group.Single(q => q.BonusPredictionType == BonusPredictionType.FirstTeamInGroup).TeamId,
                    group.Single(q => q.BonusPredictionType == BonusPredictionType.SecondTeamInGroup).TeamId,
                    worldCupGroup);
            }

            var finalMatch = matches.SingleOrDefault(q => q.MatchType == MatchType.Final);
            sum+=CalculateBonusScoreForTopNotch(
                bonusPredictions.Single(q => q.BonusPredictionType == BonusPredictionType.FirstTeamInWorldCup).TeamId,
                finalMatch, BonusPredictionType.FirstTeamInWorldCup);

            sum += CalculateBonusScoreForTopNotch(
                bonusPredictions.Single(q => q.BonusPredictionType == BonusPredictionType.SecondTeamInWorldCup).TeamId,
                finalMatch, BonusPredictionType.SecondTeamInWorldCup);

            var playoffMatch = matches.SingleOrDefault(q => q.MatchType == MatchType.ThirdPlacePlayOff);
            sum += CalculateBonusScoreForTopNotch(
                bonusPredictions.Single(q => q.BonusPredictionType == BonusPredictionType.ThirdTeamInWorldCup).TeamId,
                finalMatch, BonusPredictionType.ThirdTeamInWorldCup);


            return sum;
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

            if (match.HomeTeamScore.MatchResult is null
                || match.AwayTeamScore.MatchResult is null)
                return (null, null, 0);

            if (matchPrediction == null)
                return (null, null, 0);

            if (matchPrediction.HomeTeamScore.MatchResult is null
                || matchPrediction.AwayTeamScore.MatchResult is null)
                return (null, null, 0);

            var matchPredictionType = GetMatchPredictionType(match, matchPrediction);
            var penaltyPredictionType = GetPenaltyPredictionType(match, matchPrediction);

            if (matchPrediction.MatchResultType!=match.MatchResultType)
                return (matchPredictionType, penaltyPredictionType, 0);

            if (matchPrediction.MatchWinner?.Id!=match.MatchWinner?.Id)
                return (matchPredictionType, penaltyPredictionType, 0);

            var matchRule = GetPredictionRule(match);

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

            if (match.HomeTeamScore.MatchResult.Value == matchPrediction.HomeTeamScore.MatchResult.Value
                && match.AwayTeamScore.MatchResult.Value==matchPrediction.AwayTeamScore.MatchResult.Value)
                predictionType = MatchPredictionType.Exact;
            else if (match.GoalDifference == matchPrediction.GoalDifference)
                predictionType= MatchPredictionType.GoalDifference;
            else if (match.MatchWinner != null && matchPrediction.MatchWinner != null
                && match.MatchWinner.Id == matchPrediction.MatchWinner.Id)
                predictionType= MatchPredictionType.MatchWinner;
            else if(match.MatchResultType.Value== MatchResultType.Withdraw &&
                match.MatchResultType==matchPrediction.MatchResultType)
                predictionType = MatchPredictionType.MatchWinner;
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

        internal int CalculateBonusScoreForGroup(long? winnerTeamId, long? runnerupTeamId, WorldCupGroup worldCupGroup)
        {
            var score = 0;

            if (winnerTeamId.HasValue && worldCupGroup.WinnerId == winnerTeamId)
                score++;
            if (runnerupTeamId.HasValue && worldCupGroup.RunnerupId == runnerupTeamId)
                score++;
            if (score == 2)
                score++;

            return score;
        }

        internal int CalculateBonusScoreForTopNotch(long? teamId, Match match, BonusPredictionType bonusPredictionType)
        {
            if (match == null)
                return 0;

            if (!match.HomeTeamScore.MatchResult.HasValue)
                return 0;

            var winnderId = 0L;

            if (match.MatchWinner != null)
            {
                winnderId = match.MatchWinner.Id;
            }
            else if (match.PenaltyWinner!=null)
            {
                winnderId = match.PenaltyWinner.Id;
            }

            if (teamId != winnderId)
                return 0;

            if (bonusPredictionType == BonusPredictionType.FirstTeamInWorldCup)
                return 6;
            if (bonusPredictionType == BonusPredictionType.SecondTeamInGroup)
                return 5;
            if (bonusPredictionType == BonusPredictionType.ThirdTeamInWorldCup)
                return 4;

            return 0;
        }


    }
}
