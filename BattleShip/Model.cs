using Newtonsoft.Json;
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
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Room
    {
        public string Status { get; set; }
        public string Turn { get; set; }
        public PlayerData Player1 { get; set; }
        public PlayerData Player2 { get; set; }
    }
    public class Shot
    {
        public string Attacker { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Time { get; set; }
    }
    public class ShotResponse
    {
        public string Responder { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Result { get; set; }
    }
}
