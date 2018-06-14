using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.ViewModels.QueryResult
{
    public class GroupBonusPredictionStatQueryResult
    {
        public GroupBonusPredictionStatQueryResult()
        {
        }

        public string UserName { get; set; }
        public string UserImageUrl { get; set; }
        public string FirstTeamName { get; set; }
        public string SecondTeamName { get; set; }
        public int Score { get; set; }
    }
}
