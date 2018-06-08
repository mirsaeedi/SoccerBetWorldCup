using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.Data.Models
{
    public class BetGroupBonusPredictionRule : Entity
    {
        public long BetGroupId { get; set; }
        public BetGroup BetGroup { get; set; }
        public short FirstTeamPredictionInGroupScore { get; set; }
        public short SecondTeamPredictionInGroupScore { get; set; }
        public short FirstTeamPredictionInWorldCupScore { get; set; }
        public short SecondTeamPredictionInWorldCupScore { get; set; }
        public short ThirdTeamPredictionInWorldCupScore { get; set; }
    }
}
