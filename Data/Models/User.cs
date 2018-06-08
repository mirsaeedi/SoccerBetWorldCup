using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerBet.Data.Models
{
    public class User:IdentityUser<long>
    {
        public string Name { get; set; }
        public string ImageUrl { get; internal set; }
    }
}
