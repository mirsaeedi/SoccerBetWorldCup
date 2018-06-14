using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoccerBet.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.Data
{
    public class SoccerBetDbContext:IdentityDbContext<User,Role,long>
    {
        
        public DbSet<BonusPrediction> BonusPredictions { get; set; }
        public DbSet<BetGroupBonusPredictionRule> BetGroupBonusPredictionRules { get; set; }
        public DbSet<BetGroupMatchPredictionRule> BetGroupMatchPredictionRules { get; set; }
        public DbSet<Stadium> Stadiums { get; set; }
        public DbSet<WorldCupGroup> WorldCupGroups { get; set; }
        public DbSet<MatchPrediction> MatchPredictions { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<UserBetGroup> UserBetGroups { get; set; }
        public DbSet<BetGroup> BetGroups { get; set; }
        public SoccerBetDbContext(DbContextOptions<SoccerBetDbContext> options): base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Match>()
                .OwnsOne(p => p.HomeTeamScore);

            builder.Entity<Match>()
                .OwnsOne(p => p.AwayTeamScore);

            builder.Entity<MatchPrediction>()
                .OwnsOne(p => p.HomeTeamScore);

            builder.Entity<MatchPrediction>()
                .OwnsOne(p => p.AwayTeamScore);
        }
    }
}
