using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.ViewModels.QueryResult
{
    public class BonusPredictionsScoreQueryResult
    {
        public BonusPredictionsScoreQueryResult()
        {
        }

        public string BonusType { get; set; }
        public int Score { get; set; }
    }
}
