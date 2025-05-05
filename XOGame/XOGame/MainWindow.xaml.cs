using System.Windows;

namespace XOGame;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void PlayButton_Click(object sender, RoutedEventArgs e)
    {
        PlayButton.Visibility = Visibility.Collapsed;
        ModeSelectionPanel.Visibility = Visibility.Visible;
        MusicToggleButton.Visibility = Visibility.Collapsed;

        if (AudioManager.IsPlaying)
        {
            AudioManager.StartMusic(); 
        }

    }

    private void ToggleMusic_Click(object sender, RoutedEventArgs e)
    {
        AudioManager.HasUserToggledMusic = true;

        if (AudioManager.IsPlaying)
        {
            AudioManager.MenuMusicPlayer.Pause();
            AudioManager.IsPlaying = false;

            MusicIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.VolumeOff; 
        }
        else
        {
            AudioManager.MenuMusicPlayer.Play();
            AudioManager.IsPlaying = true;

            MusicIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.VolumeHigh; 
        }
    }


    private void OnePlayerButton_Click(object sender, RoutedEventArgs e)
    {
        SelectDifficultyWindow difficultyWindow = new SelectDifficultyWindow();
        difficultyWindow.Show();
        this.Close();
    }


    private void TwoPlayerButton_Click(object sender, RoutedEventArgs e)
    {
        GameWindow gameWindow = new GameWindow(false);
        gameWindow.Show();
        this.Close();
    }

    private void BackToMainMenu_Click(object sender, RoutedEventArgs e)
    {
        ModeSelectionPanel.Visibility = Visibility.Collapsed;
        PlayButton.Visibility = Visibility.Visible;
        MusicToggleButton.Visibility = Visibility.Visible;
    }

}