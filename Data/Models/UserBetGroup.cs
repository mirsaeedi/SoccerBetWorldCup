using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.Data.Models
{
    public class UserBetGroup:Entity
    {
        public long BetGroupId { get; set; }
        public BetGroup BetGroup { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
