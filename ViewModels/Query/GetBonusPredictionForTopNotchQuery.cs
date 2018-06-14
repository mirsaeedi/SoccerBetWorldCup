using SoccerBet.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.ViewModels.Query
{
    public class GetBonusPredictionForTopNotchQuery
    {
        public long BetGroupId { get; set; }
        public BonusPredictionType BonusPredictionType { get; set; }
    }
}
