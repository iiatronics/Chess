using System;
using System.Windows.Media;

namespace ChessGame.UI.Services
{
    public static class AudioService
    {
        private static MediaPlayer _player = new MediaPlayer();
        private static bool _isMuted = false;

        public static void PlayMusic()
        {
            try
            {
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sounds", "menu.mp3");
                _player.Open(new Uri(path));
                _player.Volume = 0.1; 
                

                _player.MediaEnded += (s, e) => 
                {
                    _player.Position = TimeSpan.Zero;
                    _player.Play();
                };

                _player.Play();
            }
            catch {  }
        }

        public static void ToggleSound(bool isEnabled)
        {
            _isMuted = !isEnabled;
            _player.Volume = _isMuted ? 0 : 0.3;
        }

        public static void SetVolume(double volume)
        {
            if (!_isMuted)
            {
                _player.Volume = volume;
            }
        }
    }
}