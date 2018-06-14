using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.ViewModels.Query
{
    public class GetBonusPredictionForGroupQuery
    {
        public long BetGroupId { get; set; }
        public string GroupName { get; set; }
    }
}
