﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.ViewModels.Query
{
    public class GetMatchPredictionQuery
    {
        public long MatchId { get; set; }
        public long BetGroupId { get; set; }
    }
}
