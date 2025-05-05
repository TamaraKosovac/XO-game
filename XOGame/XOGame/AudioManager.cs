using System.Windows.Media;

namespace XOGame
{
    public static class AudioManager
    {
        public static MediaPlayer MenuMusicPlayer { get; private set; } = new MediaPlayer();
        public static bool IsPlaying { get; set; } = true; 
        public static bool HasUserToggledMusic { get; set; } = false; 


        public static void StartMusic()
        {
            if (HasUserToggledMusic && !IsPlaying)
                return; // korisnik je rekao da NEĆE muziku → poštuj

            if (MenuMusicPlayer.Source == null)
            {
                MenuMusicPlayer.Open(new Uri("Resources/music.mp3", UriKind.Relative));
                MenuMusicPlayer.Volume = 0.3;
                MenuMusicPlayer.MediaEnded += (s, e) => MenuMusicPlayer.Position = TimeSpan.Zero;
            }

            MenuMusicPlayer.Play();
            IsPlaying = true;
        }



        public static void ToggleMusic()
        {
            if (IsPlaying)
            {
                MenuMusicPlayer.Pause();
                IsPlaying = false;
            }
            else
            {
                MenuMusicPlayer.Play();
                IsPlaying = true;
            }
        }
    }
}
