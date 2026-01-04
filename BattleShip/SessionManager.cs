using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    internal class SessionManager
    {
        public static string CurrentUserID { get; private set; }
        public static string CurrentToken { get; private set; }
        public static string CurrentEmail { get; private set; }
        public static void SetSession(string userId, string token, string email = null)
        {
            CurrentUserID = userId;
            CurrentToken = token;
            if (email != null)
            {
                CurrentEmail = email;
            }
        }

        public static void ClearSession()
        {
            CurrentUserID = null;
            CurrentToken = null;
            CurrentEmail = null;
        }

        public static bool IsLoggedIn => CurrentUserID != null;
    }
}
