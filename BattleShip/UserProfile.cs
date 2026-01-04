using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class UserProfile
    {
        public string Email { get; set; } //gmail
        public string DisplayName { get; set; }//tên
        public int AvatarId { get; set; }//avatar có sẵn từ một đến tám
        public int TotalWins { get; set; }
        public int TotalLosses { get; set; }
        public string DateOfBirth { get; set; } // Lưu dưới dạng chuỗi (dd/MM/yyyy) 
        public string Gender { get; set; } // Nam nữ hoặc khác

    }
}
