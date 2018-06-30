using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SoccerBetDbContext _dbContext;

        public UserController(SoccerBetDbContext dbContext, UserManager<User> userManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpGet("me/matches")]
        public async Task<IActionResult> GetMyMatchPredictions(GetMyMatchPredictionsQuery query)
        {
            var userBetGroup = await GetCurrentUserBetGroup(query.BetGroupId);
            var scoreCalculator= await GetScoreCalculator(query.BetGroupId);
            var matches = await GetAllWorldCupMatches();
            var userMatchPredictions = (await GetUserMatchPredictions(userBetGroup.Id))
                .ToDictionary(q => q.MatchId);

            var result = matches.Select(match => new
            {
                match.Id,
                WorldCupGroupName = match.HomeTeamScore.Team.WorldCupGroup.Name,
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
                HomeTeamPredictionResult = userMatchPredictions.ContainsKey(match.Id) ? userMatchPredictions[match.Id]?.HomeTeamScore.MatchResult : null,
                AwayTeamPredictionResult = userMatchPredictions.ContainsKey(match.Id) ? userMatchPredictions[match.Id]?.AwayTeamScore.MatchResult : null,
                HomeTeamPredictionPenaltyResult = userMatchPredictions.ContainsKey(match.Id) ? userMatchPredictions[match.Id]?.HomeTeamScore?.PenaltyResult : null,
                AwayTeamPredictionPenaltyResult = userMatchPredictions.ContainsKey(match.Id) ? userMatchPredictions[match.Id]?.AwayTeamScore?.PenaltyResult : null,
                CityName = match.Stadium.City,
                StadiumName = match.Stadium.Name,
                StadiumImageUrl = match.Stadium.Image,
                MatchDateTime = match.DateTime,
                match.MatchHasStarted,
                match.MatchType,
                Score = scoreCalculator.CalculateMatchScore(match, userMatchPredictions.ContainsKey(match.Id) ? userMatchPredictions[match.Id] : null)
                .score
            }).ToArray();

            return Ok(result);
        }

        private async Task<MatchPrediction[]> GetUserMatchPredictions(long userBetGroupId)
        {
            var matchPredictions = await _dbContext.MatchPredictions
                            .Where(q => q.UserBetGroup.Id == userBetGroupId)
                            .Include(m => m.HomeTeamScore.Team)
                            .Include(m => m.AwayTeamScore.Team)
                            .ToArrayAsync();

            return matchPredictions;
        }

        private async Task<Match[]> GetAllWorldCupMatches()
        {
            return await _dbContext.Matches
                .Include(m => m.HomeTeamScore.Team.WorldCupGroup)
                .Include(m => m.AwayTeamScore.Team.WorldCupGroup)
                .Include(m => m.Stadium)
                .ToArrayAsync();
        }

        private async Task<ScoreCalculator> GetScoreCalculator(long betGroupId)
        {
            var matchRules = await _dbContext
                .BetGroupMatchPredictionRules
                .Where(q => q.BetGroupId == betGroupId)
                .ToArrayAsync();

            var bonusRules = await _dbContext
                .BetGroupBonusPredictionRules
                .Where(q => q.BetGroupId == betGroupId)
                .ToArrayAsync();

            var scoreCalculator = new ScoreCalculator(matchRules, bonusRules);

            return scoreCalculator;
        }

        private async Task<UserBetGroup> GetCurrentUserBetGroup(long betGroupId)
        {
            var user = await GetCurrentUser();

            var userBetGroup = await _dbContext.UserBetGroups
                .SingleAsync(q => q.BetGroupId == betGroupId && q.UserId == user.Id);

            return userBetGroup;
        }

        private async Task<User> GetCurrentUser()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return await _userManager.FindByNameAsync(username);
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinBetGroup([FromBody] JoinBetGroupCommand command)
        {
            var user = await GetCurrentUser();

            var betGroup = await _dbContext
                .BetGroups
                .SingleOrDefaultAsync(q => q.GroupCode == command.GroupCode);

            if (betGroup == null)
            {
                return NotFound();
            }

            var existingBetUserGroup = await _dbContext
                .UserBetGroups.
                SingleOrDefaultAsync(q => q.BetGroupId == betGroup.Id && q.UserId == user.Id);

            if (existingBetUserGroup != null)
            {
                return Ok(new { betGroup.Name, betGroup.Id });
            }

            var newBetUserGroup = new UserBetGroup()
            {
                BetGroupId = betGroup.Id,
                UserId = user.Id
            };

            _dbContext.Add(newBetUserGroup);

            await AddEmptyBonusPredictionsForUser(newBetUserGroup);

            await _dbContext.SaveChangesAsync();

            return Ok(new { betGroup.Name, betGroup.Id });
        }

        private async Task AddEmptyBonusPredictionsForUser(UserBetGroup newBetUserGroup)
        {
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
        }

        [HttpGet("me/bet-groups")]
        public async Task<IActionResult> GetMyBetGroups()
        {
            var user = await GetCurrentUser();

            var myBetGroups = await _dbContext.UserBetGroups
                .Where(q => q.UserId == user.Id)
                .Select(m => m.BetGroup)
                .ToArrayAsync();

            return Ok(myBetGroups);
        }

        [HttpPost("match/{matchId}/prediction")]
        public async Task<IActionResult> PredictMatch([FromBody] MatchPredictionCommand command)
        {
            var userBetGroup = await GetCurrentUserBetGroup(command.BetGroupId);
            var match = await GetMatchWithId(command.MatchId);

            if (match.DateTime.LocalDateTime < DateTime.Now)
            {
                return Unauthorized();
            }

            var prediction = await _dbContext.MatchPredictions
                .SingleOrDefaultAsync(q => q.MatchId == command.MatchId && q.UserBetGroupId == userBetGroup.Id);

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

        private async Task<Match> GetMatchWithId(long matchId)
        {
            return await _dbContext
                .Matches
                .SingleOrDefaultAsync(q => q.Id == matchId);
        }

        [HttpGet("match/{matchId}/predictions")]
        public async Task<IActionResult> GetMatchPredictions(GetMatchPredictionQuery query)
        {
            var match = await GetMatchWithId(query.MatchId);

            if (match.DateTime.LocalDateTime > DateTime.Now)
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
            var userBetGroup = await GetCurrentUserBetGroup(query.BetGroupId);

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

        [HttpGet("bonus-predictions/scores")]
        public async Task<IActionResult> GetBonusPredictionsScores(GetBonusPredictionQuery query)
        {
            var result = new List<BonusPredictionsScoreQueryResult>();

            var userBetGroup = await GetCurrentUserBetGroup(query.BetGroupId);

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

            var groups = await _dbContext.WorldCupGroups.ToArrayAsync();
            var scoreCalculator = new ScoreCalculator(null, null);

            foreach (var group in groups)
            {
                var groupPredictions = bonusPredictions
                    .Where(q => q.WorldCupGroupId == group.Id)
                    .ToArray();

                var winner = groupPredictions
                    .Single(q => q.BonusPredictionType == BonusPredictionType.FirstTeamInGroup).TeamId;

                var runnerup = groupPredictions
                    .Single(q => q.BonusPredictionType == BonusPredictionType.SecondTeamInGroup).TeamId;
                
                var score= scoreCalculator
                    .CalculateBonusScoreForGroup(winner,runnerup, group);

                var record = new BonusPredictionsScoreQueryResult()
                {
                    Score = score,
                    BonusType=group.Name
                };

                result.Add(record);

            }

            var match = await _dbContext
                .Matches
                .SingleOrDefaultAsync(q => q.MatchType == MatchType.Final);

            var stat = new BonusPredictionsScoreQueryResult()
            {
                Score = scoreCalculator.CalculateBonusScoreForTopNotch
                (
                    bonusPredictions.Single(q => q.BonusPredictionType == BonusPredictionType.FirstTeamInWorldCup).TeamId,
                    match, BonusPredictionType.FirstTeamInWorldCup),
                    BonusType = BonusPredictionType.FirstTeamInWorldCup.ToString()
            };

            result.Add(stat);


            stat = new BonusPredictionsScoreQueryResult()
            {
                Score = scoreCalculator.CalculateBonusScoreForTopNotch
                (
                    bonusPredictions.Single(q => q.BonusPredictionType == BonusPredictionType.SecondTeamInWorldCup).TeamId,
                    match, BonusPredictionType.SecondTeamInWorldCup),
                    BonusType = BonusPredictionType.SecondTeamInWorldCup.ToString()
            };

            result.Add(stat);

            match = await _dbContext
                .Matches
                .SingleOrDefaultAsync(q => q.MatchType == MatchType.ThirdPlacePlayOff);

            stat = new BonusPredictionsScoreQueryResult()
            {
                Score = scoreCalculator.CalculateBonusScoreForTopNotch
                (
                    bonusPredictions.Single(q => q.BonusPredictionType == BonusPredictionType.ThirdTeamInWorldCup).TeamId,
                    match, BonusPredictionType.ThirdTeamInWorldCup),
                BonusType = BonusPredictionType.ThirdTeamInWorldCup.ToString()
            };

            result.Add(stat);

            return Ok(result);
        }

        [HttpPost("bonus-predictions/{bonusPredictionId}")]
        public async Task<IActionResult> SaveBonusPrediction([FromBody] SaveBonusPredictionCommand command)
        {
            var bonusPrediction = await _dbContext.BonusPredictions
                .SingleOrDefaultAsync(q => q.Id == command.BonusPredictionId);

            if (bonusPrediction.WorldCupGroupId != null)
            {
                var deadlineForSubmit =await _dbContext
                    .Matches
                    .Where(q=>q.MatchType==MatchType.Group)
                    .MinAsync(q=>q.DateTime);

                if (DateTime.Now > deadlineForSubmit.LocalDateTime)
                    return Unauthorized();
            }
            else
            {
                var firstMatchDateTime = (await _dbContext
                                    .Matches
                                    .Where(q => q.MatchType == MatchType.RoundOf16)
                                    .MinAsync(q => q.DateTime));

                if (firstMatchDateTime!=null && DateTime.Now > firstMatchDateTime.ToLocalTime())
                    return Unauthorized();
            }

            if (command.TeamId != null)
            {

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
                       && (q.WorldCupGroupId == bonusPrediction.WorldCupGroupId));

                    if (isAnotherPredictionWithThisTeam)
                        return BadRequest();
                }
            }

            bonusPrediction.TeamId = command.TeamId;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("teams")]
        public async Task<IActionResult> GetTeams()
        {
            var teams = await _dbContext.
                Teams
                .Select(m => new
                {
                    m.Id,
                    m.WorldCupGroupId,
                    TeamName = m.Name,
                    WorldCupGroupName = m.WorldCupGroup.Name
                })
                .ToArrayAsync();

            return Ok(teams);
        }

        [HttpGet("match/{matchId}/stats")]
        public async Task<IActionResult> GetMatchStats(GetMatchStatsQuery query)
        {
            var scoreCalculator = await GetScoreCalculator(query.BetGroupId);
            var matchPredictions = await GetMatchPredictions(query.MatchId,query.BetGroupId);
            var match = await GetMatchWithId(query.MatchId);

            var result = matchPredictions.Select(m => new
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

        private async Task<MatchPrediction[]> GetMatchPredictions(long matchId, long betGroupId)
        {
            return await _dbContext.MatchPredictions
                .Include(q => q.HomeTeamScore.Team)
                .Include(q => q.AwayTeamScore.Team)
                .Include(q => q.UserBetGroup.User)
                .Where(q => q.MatchId == matchId && q.UserBetGroup.BetGroupId == betGroupId)
                .ToArrayAsync();
        }

        [HttpGet("ranks")]
        public async Task<IActionResult> GetUserRanks(GetUserRanksQuery query)
        {
            var scoreCalculator = await GetScoreCalculator(query.BetGroupId);
            var usersOfBetGroup = await GetMembersOfBetGroup(query.BetGroupId);
            var matches = await GetAllWorldCupMatches();

            var result = new List<UserRankQueryResult>();

            foreach (var user in usersOfBetGroup)
            {
                var userBetGroup = await _dbContext
                    .UserBetGroups
                    .SingleAsync(q => q.BetGroupId == query.BetGroupId && q.UserId == user.Id);

                var userMatchPredictions = await GetUserMatchPredictions(userBetGroup.Id);

                var userBonusPredictions = await GetUserBonusPredictions(userBetGroup.Id);

                var userMatchPredictionScore = scoreCalculator
                    .CalculateTotalMatchPredictionScoreForUser(user, matches, userMatchPredictions);

                var worldCupGroups = await _dbContext.WorldCupGroups.ToArrayAsync();

                var userBonusPredictionScore = scoreCalculator
                    .CalculateTotalBonusPredictionScoreForUser(user, matches,worldCupGroups, userBonusPredictions);

                result.Add(new UserRankQueryResult()
                {
                    UserName = user.Name,
                    UserImageUrl = user.ImageUrl,
                    MatchScore = userMatchPredictionScore.Sum(q => q.Score),
                    BonusScore = userBonusPredictionScore,
                    CorrectPredictions = userMatchPredictionScore.Count(q => q.MatchPredictionType == MatchPredictionType.Exact),
                    GoalDifferencePredictions = userMatchPredictionScore.Count(q => q.MatchPredictionType == MatchPredictionType.GoalDifference),
                    MatchWinnerPredictions = userMatchPredictionScore.Count(q => q.MatchPredictionType == MatchPredictionType.MatchWinner),
                    WrongPredictions = userMatchPredictionScore.Count(q => q.MatchPredictionType == MatchPredictionType.Wrong)
                });

            }

            return Ok(result.OrderByDescending(q => q.TotalScore));
        }

        private async Task<BonusPrediction[]> GetUserBonusPredictions(long userBetGroupId)
        {
            return await _dbContext
                   .BonusPredictions
                   .Where(q => q.UserBetGroupId == userBetGroupId)
                   .ToArrayAsync();
        }

        private async Task<User[]> GetMembersOfBetGroup(long betGroupId)
        {
            return await _dbContext.UserBetGroups
                .Where(q => q.BetGroupId == betGroupId)
                .Select(m => m.User)
                .ToArrayAsync();
        }

        [HttpGet("me/status")]
        public async Task<IActionResult> GetUserStatus(GetUserStatusQuery query)
        {
            var scoreCalculator = await GetScoreCalculator(query.BetGroupId);
            var usersOfBetGroup = await GetMembersOfBetGroup(query.BetGroupId);
            var matches = await GetAllWorldCupMatches();

            var result = new List<UserRankQueryResult>();

            foreach (var user in usersOfBetGroup)
            {
                var userBetGroup = await _dbContext
                    .UserBetGroups
                    .SingleAsync(q => q.BetGroupId == query.BetGroupId && q.UserId == user.Id);

                var userMatchPredictions = await GetUserMatchPredictions(userBetGroup.Id);

                var userBonusPredictions = await GetUserBonusPredictions(userBetGroup.Id);

                var userMatchPredictionScore = scoreCalculator
                    .CalculateTotalMatchPredictionScoreForUser(user, matches, userMatchPredictions);

                var worldCupGroups = await _dbContext.WorldCupGroups.ToArrayAsync();

                var userBonusPredictionScore = scoreCalculator
                    .CalculateTotalBonusPredictionScoreForUser(user, matches,worldCupGroups, userBonusPredictions);

                result.Add(new UserRankQueryResult()
                {
                    UserId=user.Id,
                    UserName = user.Name,
                    UserImageUrl = user.ImageUrl,
                    MatchScore = userMatchPredictionScore.Sum(q => q.Score),
                    BonusScore = userBonusPredictionScore,
                    CorrectPredictions = userMatchPredictionScore.Count(q => q.MatchPredictionType == MatchPredictionType.Exact),
                    GoalDifferencePredictions = userMatchPredictionScore.Count(q => q.MatchPredictionType == MatchPredictionType.GoalDifference),
                    MatchWinnerPredictions = userMatchPredictionScore.Count(q => q.MatchPredictionType == MatchPredictionType.MatchWinner),
                    WrongPredictions = userMatchPredictionScore.Count(q => q.MatchPredictionType == MatchPredictionType.Wrong)
                });

            }

            var currentUser = await GetCurrentUser();

            var sortedArray = result
                .OrderByDescending(q => q.TotalScore)
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

        [HttpGet("bonus-predictions/groups/{groupName}")]
        public async Task<IActionResult> GetBonusPredictionForGroup(GetBonusPredictionForGroupQuery query)
        {

            var deadlineForSubmit = await _dbContext
                    .Matches
                    .Where(q => q.MatchType == MatchType.Group)
                    .MinAsync(q => q.DateTime);

            if (DateTime.Now < deadlineForSubmit.LocalDateTime)
                return Unauthorized();

            var userBetGroup = await GetCurrentUserBetGroup(query.BetGroupId);
            var scoreCalculator = await GetScoreCalculator(query.BetGroupId);
            var group = await GetGroupByName(query.GroupName);
            var allBonusPredictionsForGroup = await GetBonusPredictionsForGroup(group.Id, query.BetGroupId);

            var result = new List<GroupBonusPredictionStatQueryResult>();

            foreach (var userPredictionsForUser in allBonusPredictionsForGroup.GroupBy(q => q.UserBetGroupId))
            {
                var winnerTeamPrediction = userPredictionsForUser
                    .Single(q => q.BonusPredictionType == BonusPredictionType.FirstTeamInGroup);

                var runnerupTeamPrediction = userPredictionsForUser
                    .Single(q => q.BonusPredictionType == BonusPredictionType.SecondTeamInGroup);

                var score = scoreCalculator
                    .CalculateBonusScoreForGroup
                    (winnerTeamPrediction.TeamId, runnerupTeamPrediction.TeamId, group);

                var record = new GroupBonusPredictionStatQueryResult()
                {
                    UserName = winnerTeamPrediction.UserBetGroup.User.Name,
                    UserImageUrl = winnerTeamPrediction.UserBetGroup.User.ImageUrl,
                    FirstTeamName = winnerTeamPrediction.Team?.Name,
                    SecondTeamName = runnerupTeamPrediction.Team?.Name,
                    Score=score
                };

                result.Add(record);
            }

            return Ok(result);   
        }

        private async Task<BonusPrediction[]> GetBonusPredictionsForGroup(long groupId, long betGroupId)
        {
            return await _dbContext.BonusPredictions
                .Where(q => q.WorldCupGroup.Id == groupId
                && q.UserBetGroup.BetGroupId == betGroupId)
                .Include(m => m.UserBetGroup.User)
                .Include(m => m.Team)
                .ToArrayAsync();
        }

        private async Task<WorldCupGroup> GetGroupByName(string groupName)
        {
            return await _dbContext
                .WorldCupGroups
                .SingleAsync(q => q.Name == groupName);
        }

        private async Task<Match[]> GetGroupMatches(long groupId)
        {
            return await _dbContext.Matches
               .Where(q => q.MatchType == MatchType.Group
               && q.HomeTeamScore.Team.WorldCupGroup.Id == groupId)
               .ToArrayAsync();
        }

        [HttpGet("bonus-predictions/top-notchs/{predictionType}")]
        public async Task<IActionResult> GetBonusPredictionForTopNotch(GetBonusPredictionForTopNotchQuery query)
        {
            var firstMatchDateTime = await _dbContext
                                    .Matches
                                    .Where(q => q.MatchType == MatchType.RoundOf16)
                                    .MinAsync(q => q.DateTime);

            if (firstMatchDateTime == null || (DateTime.Now < firstMatchDateTime.LocalDateTime))
                return Unauthorized();

            var userBetGroup = await GetCurrentUserBetGroup(query.BetGroupId);
            var scoreCalculator = await GetScoreCalculator(query.BetGroupId);
            var bonusPredictionsForPredictionType = await GetBonusPredictionsFor(query.BonusPredictionType, query.BetGroupId);
            MatchType matchType = GetMatchTypeBasedOfPredictionType(query.BonusPredictionType);

            var match = await _dbContext.Matches
                .SingleOrDefaultAsync(q => q.MatchType == matchType);

            var result = new List<GroupBonusPredictionStatQueryResult>();

            foreach (var bonusPrediction in bonusPredictionsForPredictionType)
            {
               var score = scoreCalculator
                    .CalculateBonusScoreForTopNotch(bonusPrediction.TeamId, match, query.BonusPredictionType);

                var record = new GroupBonusPredictionStatQueryResult()
                {
                    UserName = bonusPrediction.UserBetGroup.User.Name,
                    UserImageUrl = bonusPrediction.UserBetGroup.User.ImageUrl,
                    FirstTeamName = bonusPrediction.Team?.Name,
                    Score=score
                };

                result.Add(record);
            }
            return Ok(result);
        }

        private MatchType GetMatchTypeBasedOfPredictionType(BonusPredictionType bonusPredictionType)
        {
            if (bonusPredictionType == BonusPredictionType.FirstTeamInWorldCup)
                return MatchType.Final;
            if (bonusPredictionType == BonusPredictionType.SecondTeamInWorldCup)
                return MatchType.Final;
            if (bonusPredictionType == BonusPredictionType.ThirdTeamInWorldCup)
               return MatchType.ThirdPlacePlayOff;

            return MatchType.Group;
        }

        private async Task<BonusPrediction[]> GetBonusPredictionsFor(BonusPredictionType bonusPredictionType, long betGroupId)
        {
            return await _dbContext.BonusPredictions
               .Where(q => q.BonusPredictionType == bonusPredictionType && q.UserBetGroup.BetGroupId == betGroupId)
               .Include(m=>m.UserBetGroup.User)
               .Include(m => m.Team)
               .ToArrayAsync();
        }
    } 
}