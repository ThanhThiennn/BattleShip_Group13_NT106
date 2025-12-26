using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class RoomData
    {
        public string GameStatus { get; set; } = "waiting";
        public string Turn { get; set; } = "p1";
        public PlayerData P1 { get; set; }
        public PlayerData P2 { get; set; }
        public ActionData Actions { get; set; } = new ActionData();
    }

    // Class chứa thông tin chi tiết từng người chơi
    public class PlayerData
    {
        public string Name { get; set; }
        public bool IsReady { get; set; } = false;
        public int TotalHits { get; set; } = 0;
        // Có thể thêm danh sách trạng thái chìm tàu ở đây nếu cần
    }

    // Class quản lý các hành động bắn phá
    public class ActionData
    {
        public ShotData LastShot { get; set; } = new ShotData();
    }

    // Class chi tiết về một cú bắn
    public class ShotData
    {
        public string By { get; set; } = "";    // "p1" hoặc "p2"
        public int X { get; set; } = -1;
        public int Y { get; set; } = -1;
        public string Type { get; set; } = "None"; // "Pending", "Hit", "Miss"
    }
}
