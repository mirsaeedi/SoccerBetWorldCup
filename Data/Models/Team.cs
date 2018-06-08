using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.Data.Models
{
    public class Team:Entity
    {
        public string Name { get; set; }
        public string FifaCode { get; set; }
        public string FlagUrl { get; set; }
    }
}
