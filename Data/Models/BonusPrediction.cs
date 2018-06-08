using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.Data.Models
{
    public class BonusPrediction:Entity
    {
        public long? TeamId { get; set; }
        public Team Team { get; set; }
        public long? WorldCupGroupId { get; set; }
        public WorldCupGroup WorldCupGroup { get; set; }
        public long UserBetGroupId { get; set; }
        public UserBetGroup UserBetGroup { get; set; }
        public BonusPredictionType BonusPredictionType { get; set; }
    }
}
