using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.ViewModels.Command
{
    public class MatchPredictionCommand
    {
        public long MatchId { get; set; }
        public short HomeMatchResult { get; set; }
        public short AwayMatchResult { get; set; }
        public long BetGroupId { get; set; }
        public long? PenaltyWinnerTeamId { get; set; }
    }
}
