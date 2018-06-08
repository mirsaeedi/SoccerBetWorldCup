using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.Data.Models
{
    public class BetGroup:Entity
    {
        public string Name { get; set; }
        public string GroupCode { get; set; }
    }
}
