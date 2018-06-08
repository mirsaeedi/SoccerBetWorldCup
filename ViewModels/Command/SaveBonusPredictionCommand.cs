using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.ViewModels.Command
{
    public class SaveBonusPredictionCommand
    {
        public long? TeamId { get; set; }
        public long BonusPredictionId { get; set; }
        public long BetGroupId { get; set; }
    }
}
