using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.Data.Models
{
    public class TeamScore
    {
        public long TeamId { get; set; }
        public Team Team { get; set; }
        public short? MatchResult { get; set; }
        public short? PenaltyResult { get; set; }
    }
}
