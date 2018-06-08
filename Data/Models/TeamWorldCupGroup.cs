using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.Data.Models
{
    public class TeamWorldCupGroup:Entity
    {
        public long WorldCupGroupId { get; set; }
        public WorldCupGroup WorldCupGroup { get; set; }
        public long TeamId { get; set; }
        public Team Team { get; set; }

    }
}
