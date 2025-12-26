using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class PlayerData
    {
        public string Name { get; set; }
        public bool IsReady { get; set; }
        public int ShipsLeft { get; set; }
    }

    public class Room
    {
        public string Status { get; set; }
        public string Turn { get; set; }
        public PlayerData Player1 { get; set; }
        public PlayerData Player2 { get; set; }
    }
}
