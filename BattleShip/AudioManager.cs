using System.Media;

namespace BattleShip
{
    public static class AudioManager
    {
        private static SoundPlayer _musicPlayer;
        public static bool IsMuted { get; private set; } = false;

        public static void PlayBackgroundMusic()
        {
            // Nếu đang Mute thì không hát
            if (IsMuted) return;

            try
            {
                if (_musicPlayer == null)
                {
                    // Lưu ý: bgMusic trong Resources PHẢI LÀ FILE .WAV
                    _musicPlayer = new SoundPlayer(Properties.Resources.bg);
                }

                _musicPlayer.PlayLooping(); // Hát lặp lại
            }
            catch
            {
                // Bỏ qua lỗi nếu không tìm thấy nhạc
            }
        }

        public static void StopMusic()
        {
            if (_musicPlayer != null)
            {
                _musicPlayer.Stop();
            }
        }

        // Hàm bật tắt tiếng (Dùng cho nút bấm)
        public static void ToggleMute()
        {
            IsMuted = !IsMuted; // Đảo ngược trạng thái (Đang bật -> Tắt, Đang tắt -> Bật)

            if (IsMuted)
            {
                StopMusic(); // Nếu Mute -> Dừng nhạc
            }
            else
            {
                PlayBackgroundMusic(); // Nếu Bật lại -> Hát tiếp
            }
        }
    }
}