using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoccerBet.Data;
using SoccerBet.Data.Models;
using SoccerBet.Data.Models.Constants;
using SoccerBet.ScoreEngine;
using SoccerBet.ViewModels.Command;
using SoccerBet.ViewModels.Query;
using SoccerBet.ViewModels.QueryResult;

namespace SoccerBet.Controllers
{
    [Authorize]
    [Route("api")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SoccerBetDbContext _dbContext;
        private User _currentUser;

        public UserController(SoccerBetDbContext dbContext, UserManager<User> userManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpGet("me/matches")]
        public async Task<IActionResult> GetMyPredictions(GetMyPredictionsQuery query)
        {
            var user = await _userManager.GetUserAsync(User);

            var userBetGroup = await _dbContext.UserBetGroups
                .SingleAsync(q => q.BetGroupId == query.BetGroupId && q.UserId == user.Id);

            var matchRules = await _dbContext
                .BetGroupMatchPredictionRules
                .Where(q => q.BetGroupId == query.BetGroupId)
                .ToArrayAsync();

            var bonusRules = await _dbContext
                .BetGroupBonusPredictionRules
                .Where(q => q.BetGroupId == userBetGroup.Id)
                .ToArrayAsync();

            var scoreCalculator = new ScoreCalculator(matchRules, bonusRules);

            var matches = await _dbContext.Matches
                .Include(m => m.HomeTeamScore.Team.TeamWorldCupGroups)
                .ThenInclude(m => m.WorldCupGroup)
                .Include(m => m.AwayTeamScore.Team)
                .Include(m => m.Stadium)
                .ToArrayAsync();

            var predictions = await _dbContext.MatchPredictions
                .Where(q => q.UserBetGroup.Id == userBetGroup.Id)
                .Include(m => m.HomeTeamScore.Team)
                .Include(m => m.AwayTeamScore.Team)
                .ToDictionaryAsync(q => q.MatchId);

            var result = matches.Select(match => new
            {
                match.Id,
                WorldCupGroupName = match.HomeTeamScore.Team.TeamWorldCupGroups.First().WorldCupGroup.Name,
                HomeTeamName = match.HomeTeamScore.Team.Name,
                AwayTeamName = match.AwayTeamScore.Team.Name,
                HomeTeamFlagUrl = match.HomeTeamScore.Team.FlagUrl,
                AwayTeamFlagUrl = match.AwayTeamScore.Team.FlagUrl,
                HomeTeamId = match.HomeTeamScore.Team.Id,
                AwayTeamId = match.AwayTeamScore.Team.Id,
                HomeTeamResult = match.HomeTeamScore.MatchResult,
                AwayTeamResult = match.AwayTeamScore.MatchResult,
                HomeTeamPenaltyResult = match.HomeTeamScore.PenaltyResult,
                AwayTeamPenaltyResult = match.AwayTeamScore.PenaltyResult,
                HomeTeamPredictionResult = predictions.ContainsKey(match.Id) ? predictions[match.Id]?.HomeTeamScore.MatchResult : null,
                AwayTeamPredictionResult = predictions.ContainsKey(match.Id) ? predictions[match.Id]?.AwayTeamScore.MatchResult : null,
                HomeTeamPredictionPenaltyResult = predictions.ContainsKey(match.Id) ? predictions[match.Id]?.HomeTeamScore?.PenaltyResult : null,
                AwayTeamPredictionPenaltyResult = predictions.ContainsKey(match.Id) ? predictions[match.Id]?.AwayTeamScore?.PenaltyResult : null,
                CityName = match.Stadium.City,
                StadiumName = match.Stadium.Name,
                StadiumImageUrl = match.Stadium.Image,
                MatchDateTime = match.DateTime,
                match.MatchHasStarted,
                match.MatchType,
                Score = scoreCalculator.CalculateMatchScore(match, predictions.ContainsKey(match.Id) ? predictions[match.Id] : null)
                .score
            }).ToArray();

            return Ok(result);
        }

        [HttpPost("join")]
        [Authorize]
        public async Task<IActionResult> JoinBetGroup([FromBody] JoinBetGroupCommand command)
        {
            _currentUser = _userManager.GetUserAsync(User).Result;
            var betGroup = await _dbContext.BetGroups.SingleOrDefaultAsync(q => q.GroupCode == command.GroupCode);

            if (betGroup == null)
            {
                return null;
            }

            var existingBetUserGroup = await _dbContext.UserBetGroups.
                SingleOrDefaultAsync(q => q.BetGroupId == betGroup.Id && q.UserId == _currentUser.Id);

            if (existingBetUserGroup != null)
            {
                return null;
            }

            var newBetUserGroup = new UserBetGroup()
            {
                BetGroupId = betGroup.Id,
                UserId = _currentUser.Id
            };
            _dbContext.Add(newBetUserGroup);

            var groups = await _dbContext.WorldCupGroups.ToArrayAsync();
            foreach (var group in groups)
            {
                var firstTeamInGroupBonusPrediction = new BonusPrediction()
                {
                    BonusPredictionType = BonusPredictionType.FirstTeamInGroup,
                    UserBetGroup = newBetUserGroup,
                    WorldCupGroupId = group.Id
                };
                _dbContext.Add(firstTeamInGroupBonusPrediction);

                var secondTeamInGroupBonusPrediction = new BonusPrediction()
                {
                    BonusPredictionType = BonusPredictionType.SecondTeamInGroup,
                    UserBetGroup = newBetUserGroup,
                    WorldCupGroupId = group.Id,
                };
                _dbContext.Add(secondTeamInGroupBonusPrediction);
            }

            var firstTeamInWorldCupBonusPrediction = new BonusPrediction()
            {
                BonusPredictionType = BonusPredictionType.FirstTeamInWorldCup,
                UserBetGroup = newBetUserGroup,
            };
            _dbContext.Add(firstTeamInWorldCupBonusPrediction);

            var secondTeamInWorldCupBonusPrediction = new BonusPrediction()
            {
                BonusPredictionType = BonusPredictionType.SecondTeamInWorldCup,
                UserBetGroup = newBetUserGroup,
            };
            _dbContext.Add(secondTeamInWorldCupBonusPrediction);

            var thirdTeamInWorldCupBonusPrediction = new BonusPrediction()
            {
                BonusPredictionType = BonusPredictionType.ThirdTeamInWorldCup,
                UserBetGroup = newBetUserGroup,
            };
            _dbContext.Add(thirdTeamInWorldCupBonusPrediction);

            await _dbContext.SaveChangesAsync();

            return Ok(new { betGroup.Name, betGroup.Id });
        }

        [HttpGet("me/bet-groups")]
        public async Task<IActionResult> GetMyBetGroups()
        {
            _currentUser = _userManager.GetUserAsync(User).Result;

            var myBetGroups = await _dbContext.UserBetGroups
                .Where(q => q.UserId == _currentUser.Id)
                .Select(m => m.BetGroup)
                .ToArrayAsync();

            return Ok(myBetGroups);
        }

        [HttpPost("match/{matchId}/prediction")]
        public async Task<IActionResult> GetMyBetGroups([FromBody] MatchPredictionCommand command)
        {
            _currentUser = _userManager.GetUserAsync(User).Result;

            var match = await _dbContext
                .Matches
                .SingleOrDefaultAsync(q => q.Id == command.MatchId);

            if (match.DateTime < DateTime.Now)
            {
                return BadRequest();
            }

            var userBetGroup = await _dbContext.UserBetGroups
                .SingleAsync(q => q.BetGroupId == command.BetGroupId && q.UserId == _currentUser.Id);

            var prediction = _dbContext.MatchPredictions
                .SingleOrDefault(q => q.MatchId == command.MatchId && q.UserBetGroupId == userBetGroup.Id);

            if (prediction == null)
            {
                prediction = new MatchPrediction()
                {
                    MatchId = command.MatchId,
                    UserBetGroupId = userBetGroup.Id,
                    AwayTeamScore = new TeamScore()
                    {
                        TeamId = match.AwayTeamScore.TeamId,
                        MatchResult = command.AwayMatchResult
                    },
                    HomeTeamScore = new TeamScore()
                    {
                        TeamId = match.HomeTeamScore.TeamId,
                        MatchResult = command.HomeMatchResult
                    }
                };

                if (command.AwayMatchResult == command.HomeMatchResult
                    && command.PenaltyWinnerTeamId.HasValue)
                {
                    prediction.HomeTeamScore.PenaltyResult = (short)(
                        (command.PenaltyWinnerTeamId == prediction.HomeTeamScore.TeamId) ? 1 : 0);

                    prediction.AwayTeamScore.PenaltyResult = (short)(
                        (command.PenaltyWinnerTeamId == prediction.AwayTeamScore.TeamId) ? 1 : 0);
                }

                _dbContext.Add(prediction);
            }
            else
            {
                prediction.HomeTeamScore.MatchResult = command.HomeMatchResult;
                prediction.AwayTeamScore.MatchResult = command.AwayMatchResult;
                prediction.HomeTeamScore.PenaltyResult = null;
                prediction.AwayTeamScore.PenaltyResult = null;

                if (command.AwayMatchResult == command.HomeMatchResult
                    && command.PenaltyWinnerTeamId.HasValue)
                {
                    prediction.HomeTeamScore.PenaltyResult = (short)(
                        (command.PenaltyWinnerTeamId == prediction.HomeTeamScore.TeamId) ? 1 : 0);

                    prediction.AwayTeamScore.PenaltyResult = (short)(
                        (command.PenaltyWinnerTeamId == prediction.AwayTeamScore.TeamId) ? 1 : 0);
                }
            }

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("match/{matchId}/predictions")]
        public async Task<IActionResult> GetMatchPredictions(GetMatchPredictionQuery query)
        {
            _currentUser = _userManager.GetUserAsync(User).Result;

            var match = await _dbContext
                .Matches
                .SingleOrDefaultAsync(q => q.Id == query.MatchId);

            if (match.DateTime > DateTime.Now)
            {
                return BadRequest();
            }

            var predictions = await _dbContext
                .MatchPredictions
                .Include(m => m.UserBetGroup.User)
                .Include(m => m.AwayTeamScore)
                .Include(m => m.HomeTeamScore)
                .Where(q => q.UserBetGroup.BetGroupId == query.BetGroupId && q.MatchId == query.MatchId)
                .ToArrayAsync();

            var result = predictions.Select(m => new
            {
                UserName = m.UserBetGroup.User.Name,
                UserImageUrl = m.UserBetGroup.User.ImageUrl,
                HomeTeamPredictionResult = m.HomeTeamScore.MatchResult,
                AwayTeamPredictionResult = m.AwayTeamScore.MatchResult,
                Score = 0
            }).ToArray();

            return Ok(result);
        }

        [HttpGet("bonus-predictions")]
        public async Task<IActionResult> GetBonusPredictions(GetBonusPredictionQuery query)
        {
            _currentUser = _userManager.GetUserAsync(User).Result;

            var userBetGroup = await _dbContext.UserBetGroups
                .SingleAsync(q => q.BetGroupId == query.BetGroupId && q.UserId == _currentUser.Id);

            var bonusPredictions = await _dbContext.BonusPredictions

                .Where(q => q.UserBetGroupId == userBetGroup.Id)
                .Select(m => new
                {
                    m.TeamId,
                    m.WorldCupGroupId,
                    WorldCupGroupName = m.WorldCupGroup.Name,
                    m.Id,
                    m.BonusPredictionType
                })
                .ToArrayAsync();

            return Ok(bonusPredictions);
        }

        [HttpPost("bonus-predictions/{bonusPredictionId}")]
        public async Task<IActionResult> SaveBonusPrediction([FromBody] SaveBonusPredictionCommand command)
        {
            var bonusPrediction = await _dbContext.BonusPredictions
                .SingleOrDefaultAsync(q => q.Id == command.BonusPredictionId);

            if (bonusPrediction.BonusPredictionType == BonusPredictionType.ThirdTeamInWorldCup
                || bonusPrediction.BonusPredictionType == BonusPredictionType.SecondTeamInWorldCup
                || bonusPrediction.BonusPredictionType == BonusPredictionType.FirstTeamInWorldCup)
            {
                var isAnotherPredictionWithThisTeam = await _dbContext
                    .BonusPredictions
                    .AnyAsync(q => q.UserBetGroupId == bonusPrediction.UserBetGroupId
                    && q.TeamId == command.TeamId
                    && (q.BonusPredictionType == BonusPredictionType.FirstTeamInWorldCup
                    || q.BonusPredictionType == BonusPredictionType.SecondTeamInWorldCup
                    || q.BonusPredictionType == BonusPredictionType.ThirdTeamInWorldCup));

                if (isAnotherPredictionWithThisTeam)
                    return BadRequest();
            }
            else
            {
                var isAnotherPredictionWithThisTeam = await _dbContext
                   .BonusPredictions
                   .AnyAsync(q => q.UserBetGroupId == bonusPrediction.UserBetGroupId
                   && q.TeamId == command.TeamId
                   && (q.WorldCupGroupId==bonusPrediction.WorldCupGroupId));

                if (isAnotherPredictionWithThisTeam)
                    return BadRequest();
            }

            bonusPrediction.TeamId = command.TeamId;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("teams")]
        public async Task<IActionResult> GetTeams()
        {
            var teams = await _dbContext.
                TeamWorldCupGroups
                .Select(m => new
                {
                    m.TeamId,
                    m.WorldCupGroupId,
                    TeamName = m.Team.Name,
                    WorldCupGroupName = m.WorldCupGroup.Name
                })
                .ToArrayAsync();

            return Ok(teams);
        }

        [HttpGet("match/{matchId}/stats")]
        public async Task<IActionResult> GetMatchStats(GetMatchStatsQuery query)
        {
            var matchRules = await _dbContext
                .BetGroupMatchPredictionRules
                .Where(q => q.BetGroupId == query.BetGroupId)
                .ToArrayAsync();

            var scoreCalculator = new ScoreCalculator(matchRules, null);

            var predictions = await _dbContext.MatchPredictions
                .Include(q => q.HomeTeamScore.Team)
                .Include(q => q.AwayTeamScore.Team)
                .Include(q => q.UserBetGroup.User)
                .Where(q => q.MatchId == query.MatchId && q.UserBetGroup.BetGroupId == query.BetGroupId)
                .ToArrayAsync();

            var match = await _dbContext.Matches.SingleAsync(q => q.Id == query.MatchId);

            var result = predictions.Select(m => new
            {
                UserImageUrl = m.UserBetGroup.User.ImageUrl,
                UserName = m.UserBetGroup.User.Name,
                UserEmail = m.UserBetGroup.User.Email,
                HomeTeamResult = m.HomeTeamScore.MatchResult,
                AwayTeamResult = m.AwayTeamScore.MatchResult,
                HomeTeamPenaltyResult = m.HomeTeamScore.PenaltyResult,
                AwayTeamPenaltyResult = m.AwayTeamScore.PenaltyResult,
                PenaltyWinnerTeamName = m.PenaltyWinner?.Name,
                Score = scoreCalculator.CalculateMatchScore(match, m).score
            }).ToArray();

            return Ok(result);
        }

        [HttpGet("ranks")]
        public async Task<IActionResult> GetUserRanks(GetUserRanksQuery query)
        {
            var matchRules = await _dbContext
                .BetGroupMatchPredictionRules
                .Where(q => q.BetGroupId == query.BetGroupId)
                .ToArrayAsync();

            var bonusRules = await _dbContext
                .BetGroupBonusPredictionRules
                .Where(q => q.BetGroupId == query.BetGroupId)
                .ToArrayAsync();

            var scoreCalculator = new ScoreCalculator(matchRules, bonusRules);

            var usersOfBetGroup = await _dbContext.UserBetGroups
                .Where(q => q.BetGroupId == query.BetGroupId)
                .Select(m => m.User)
                .ToArrayAsync();

            var matches = await _dbContext
                .Matches
                .Include(q=>q.AwayTeamScore.Team)
                .Include(q => q.HomeTeamScore.Team)
                .ToArrayAsync();

            var result = new List<UserRankQueryResult>();

            foreach (var user in usersOfBetGroup)
            {
                var userBetGroup = await _dbContext
                    .UserBetGroups
                    .SingleAsync(q => q.BetGroupId == query.BetGroupId && q.UserId == user.Id);

                var userMatchPredictions =await _dbContext
                    .MatchPredictions
                    .Where(q => q.UserBetGroupId == userBetGroup.Id)
                    .ToArrayAsync();

                var userBonusPredictions = await _dbContext
                    .BonusPredictions
                    .Where(q => q.UserBetGroupId == userBetGroup.Id)
                    .ToArrayAsync();

                var userPredictionResult = scoreCalculator
                    .CalculateScoreForUser
                    (user,matches, userMatchPredictions,userBonusPredictions);

                result.Add(new UserRankQueryResult()
                {
                    UserName = user.Name,
                    UserImageUrl = user.ImageUrl,
                    MatchScore = userPredictionResult.Sum(q=>q.Score),
                    BonusScore=0,
                    CorrectPredictions=userPredictionResult.Count(q=>q.MatchPredictionType==MatchPredictionType.Exact),
                    GoalDifferencePredictions = userPredictionResult.Count(q => q.MatchPredictionType == MatchPredictionType.GoalDifference),
                    MatchWinnerPredictions = userPredictionResult.Count(q => q.MatchPredictionType == MatchPredictionType.MatchWinner),
                    WrongPredictions = userPredictionResult.Count(q => q.MatchPredictionType == MatchPredictionType.Wrong)
                });

            }

            return Ok(result
                .OrderByDescending(q => q.MatchScore).ToArray());
        }


        [HttpGet("me/status")]
        public async Task<IActionResult> GetUserStatus(GetUserStatusQuery query)
        {
            var matchRules = await _dbContext
                .BetGroupMatchPredictionRules
                .Where(q => q.BetGroupId == query.BetGroupId)
                .ToArrayAsync();

            var bonusRules = await _dbContext
                .BetGroupBonusPredictionRules
                .Where(q => q.BetGroupId == query.BetGroupId)
                .ToArrayAsync();

            var scoreCalculator = new ScoreCalculator(matchRules, bonusRules);

            var usersOfBetGroup = await _dbContext.UserBetGroups
                .Where(q => q.BetGroupId == query.BetGroupId)
                .Select(m => m.User)
                .ToArrayAsync();

            var matches = await _dbContext
                .Matches
                .Include(q => q.AwayTeamScore.Team)
                .Include(q => q.HomeTeamScore.Team)
                .ToArrayAsync();

            var result = new List<UserRankQueryResult>();

            foreach (var user in usersOfBetGroup)
            {
                var userBetGroup = await _dbContext
                    .UserBetGroups
                    .SingleAsync(q => q.BetGroupId == query.BetGroupId && q.UserId == user.Id);

                var userMatchPredictions = await _dbContext
                    .MatchPredictions
                    .Where(q => q.UserBetGroupId == userBetGroup.Id)
                    .ToArrayAsync();

                var userBonusPredictions = await _dbContext
                    .BonusPredictions
                    .Where(q => q.UserBetGroupId == userBetGroup.Id)
                    .ToArrayAsync();

                var userPredictionResult = scoreCalculator
                    .CalculateScoreForUser
                    (user, matches, userMatchPredictions, userBonusPredictions);

                result.Add(new UserRankQueryResult()
                {
                    UserId=user.Id,
                    UserName = user.Name,
                    UserImageUrl = user.ImageUrl,
                    MatchScore = userPredictionResult.Sum(q => q.Score),
                    BonusScore = 0,
                    CorrectPredictions = userPredictionResult.Count(q => q.MatchPredictionType == MatchPredictionType.Exact),
                    GoalDifferencePredictions = userPredictionResult.Count(q => q.MatchPredictionType == MatchPredictionType.GoalDifference),
                    MatchWinnerPredictions = userPredictionResult.Count(q => q.MatchPredictionType == MatchPredictionType.MatchWinner),
                    WrongPredictions = userPredictionResult.Count(q => q.MatchPredictionType == MatchPredictionType.Wrong)
                });

            }

            var currentUser = await _userManager.GetUserAsync(User);

            var sortedArray = result
                .OrderByDescending(q=>q.MatchScore)
                .ToArray();

            var userRank = Array.FindIndex(sortedArray, q => q.UserId == currentUser.Id);
            var score = sortedArray[userRank];

            var userStatusResult = new
            {
                Rank = userRank + 1,
                Score = score.BonusScore + score.MatchScore,
                sortedArray[userRank].GoalDifferencePredictions,
                sortedArray[userRank].CorrectPredictions,
                sortedArray[userRank].WrongPredictions,
                sortedArray[userRank].MatchWinnerPredictions
            };

            return Ok(userStatusResult);
        }
    }
}
