using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class UserProfile
    {
        public string Email { get; set; }

        public string DisplayName { get; set; }

        public int AvatarId { get; set; }

        public int TotalWins { get; set; }
        public int TotalLosses { get; set; }
    }
}
