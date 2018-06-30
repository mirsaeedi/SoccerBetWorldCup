using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.Data.Models
{
    public class WorldCupGroup:Entity
    {
        public string Name { get; set; }

        public Team Winner { get; set; }
        public long? WinnerId { get; set; }
        public Team Runnerup { get; set; }
        public long? RunnerupId { get; set; }

        [InverseProperty("WorldCupGroup")]
        public List<Team> Teams { get; set; }

    }
}
