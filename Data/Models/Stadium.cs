using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.Data.Models
{
    public class Stadium:Entity
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string  Image { get; set; }
    }
}
