using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace XOGame
{
    public partial class GameWindow : Window
    {
        private Button[,] buttons = new Button[3, 3];
        private string currentPlayer = "X";
        private bool isVsComputer;
        private Random random = new Random();

        private DispatcherTimer gameTimer;
        private TimeSpan elapsedTime;

        private int difficultyLevel;

        public GameWindow(bool playAgainstComputer, int level = 0)
        {
            InitializeComponent();
            isVsComputer = playAgainstComputer;
            difficultyLevel = level;
            InitializeBoard();
            StatusText.Text = isVsComputer ? "One player" : "Player X has the move";

            elapsedTime = TimeSpan.Zero;
            gameTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void ComputerMove()
        {
            switch (difficultyLevel)
            {
                case 0:
                    RandomMove();
                    break;
                case 1:
                    BlockOrRandomMove();
                    break;
                case 2:
                    BestMove();
                    break;
            }
        }

        private void RandomMove()
        {
            for (int i = 0; i < 9; i++)
            {
                int row = random.Next(0, 3);
                int col = random.Next(0, 3);
                if (string.IsNullOrEmpty(buttons[row, col].Content?.ToString()))
                {
                    buttons[row, col].Content = "O";
                    AfterComputerMove();
                    return;
                }
            }
        }

        private void BlockOrRandomMove()
        {
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (string.IsNullOrEmpty(buttons[r, c].Content?.ToString()))
                    {
                        buttons[r, c].Content = "X";
                        if (CheckWin())
                        {
                            buttons[r, c].Content = "O";
                            AfterComputerMove();
                            return;
                        }
                        buttons[r, c].Content = null;
                    }
                }
            }
            RandomMove();
        }

        private void BestMove()
        {
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (string.IsNullOrEmpty(buttons[r, c].Content?.ToString()))
                    {
                        buttons[r, c].Content = "O";
                        if (CheckWin())
                        {
                            AfterComputerMove();
                            return;
                        }
                        buttons[r, c].Content = null;
                    }
                }
            }

            BlockOrRandomMove(); 
        }

        private void AfterComputerMove()
        {
            if (CheckWin())
            {
                StatusText.Text = "You lost";
                DisableAllButtons();
                gameTimer.Stop();
                return;
            }

            if (IsBoardFull())
            {
                StatusText.Text = "Tie";
                gameTimer.Stop();
                return;
            }

            currentPlayer = "X";
            StatusText.Text = $"Player X has the move";
        }



        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
            TimerText.Text = $"\u23F1 {elapsedTime:mm\\:ss}";
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Button btn = new Button
                    {
                        FontSize = 48,
                        FontWeight = FontWeights.Bold,
                        Tag = (row, col),
                        Width = 150,
                        Height = 150,
                        Margin = new Thickness(5),
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Background = (Brush)Application.Current.Resources["ButtonBackgroundColor"]
                    };

                    btn.Click += Cell_Click;
                    buttons[row, col] = btn;
                    BoardGrid.Children.Add(btn);
                }
            }
        }

        private async void Cell_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (!string.IsNullOrEmpty(btn.Content?.ToString()))
                return;

            btn.Content = currentPlayer;

            if (CheckWin())
            {
                StatusText.Text = $"Player {currentPlayer} is the winner";
                DisableAllButtons();
                gameTimer.Stop();
                return;
            }

            if (IsBoardFull())
            {
                StatusText.Text = "Tie";
                gameTimer.Stop();
                return;
            }

            SwitchPlayer();

            if (isVsComputer && currentPlayer == "O")
            {
                await Task.Delay(300);
                ComputerMove();
            }
            else
            {
                StatusText.Text = $"Player {currentPlayer} has the move";
            }
        }

        private void SwitchPlayer()
        {
            currentPlayer = currentPlayer == "X" ? "O" : "X";
        }

        private bool CheckWin()
        {
            string[,] board = new string[3, 3];

            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    var content = buttons[r, c]?.Content;
                    board[r, c] = content != null ? content.ToString()! : string.Empty;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (!string.IsNullOrEmpty(board[i, 0]) &&
                    board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                    return true;

                if (!string.IsNullOrEmpty(board[0, i]) &&
                    board[0, i] == board[1, i] && board[1, i] == board[2, i])
                    return true;
            }

            if (!string.IsNullOrEmpty(board[0, 0]) &&
                board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                return true;

            if (!string.IsNullOrEmpty(board[0, 2]) &&
                board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                return true;

            return false;
        }

        private bool IsBoardFull()
        {
            foreach (var btn in buttons)
                if (string.IsNullOrEmpty(btn.Content?.ToString()))
                    return false;
            return true;
        }

        private void DisableAllButtons()
        {
            foreach (var btn in buttons)
                btn.IsEnabled = false;
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            BoardGrid.Children.Clear();
            buttons = new Button[3, 3];
            currentPlayer = "X";
            InitializeBoard();
            StatusText.Text = isVsComputer ? "One player" : "Player X has the move";
            elapsedTime = TimeSpan.Zero;
            TimerText.Text = "⏱ 00:00";
            gameTimer.Start();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
