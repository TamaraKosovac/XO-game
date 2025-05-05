using System.Windows;


namespace XOGame
{
    public partial class SelectDifficultyWindow : Window
    {
        public SelectDifficultyWindow()
        {
            InitializeComponent();
        }

        private void StartGame(int level)
        {
            GameWindow gameWindow = new GameWindow(true, level);
            gameWindow.Show();
            this.Close();
        }

        private void Easy_Click(object sender, RoutedEventArgs e) => StartGame(0);
        private void Medium_Click(object sender, RoutedEventArgs e) => StartGame(1);
        private void Hard_Click(object sender, RoutedEventArgs e) => StartGame(2);

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
