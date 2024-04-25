using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User _player1 = null;
        private User _player2 = null;
        private bool _isNowGame;
        private int _playersMove;
        private DbContextTicTacToe _context;


        public MainWindow()
        {
            InitializeComponent();
            _isNowGame = false;
            _playersMove = 0;
            _context = new DbContextTicTacToe();
        }

        private void FrameworkElement_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustFontSize(TextBlockPanel1.Children, 20);
            AdjustFontSize(TextBlockPanel2.Children, 20);
            AdjustFontSize(MainGame.Children, 68);
        }

        private void AdjustFontSize(UIElementCollection elements, int number)
        {
            foreach (UIElement element in elements)
            {
                if (element is Control control)
                {
                    control.FontSize = number * control.ActualHeight / 500;
                }
            }
        }


        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var name = Microsoft.VisualBasic.Interaction.InputBox("Enter a name:");
            var password = Microsoft.VisualBasic.Interaction.InputBox("Enter a password:");
            _context.Database.ExecuteSqlRaw($"INSERT INTO user_credentials (password) VALUES ('{password}')");

            var userId = _context.UserCredentials.OrderByDescending(u => u.Id).Select(u => u.Id).FirstOrDefault();
            _context.Database.ExecuteSqlRaw($"INSERT INTO Users (id, name) VALUES ({userId}, '{name}');");

            MessageBox.Show("User created, your id is " + userId);
        }

        private void ChangeName_Click(object sender, RoutedEventArgs e)
        {
            var id = Microsoft.VisualBasic.Interaction.InputBox("Enter a id:");
            var password = Microsoft.VisualBasic.Interaction.InputBox("Enter a password:");
            var query = _context.Users.FromSqlRaw(
                    $"SELECT u.id, u.name, u.win, u.lose, u.draw FROM users u INNER JOIN user_credentials on u.id = user_credentials.id WHERE user_credentials.id = {id} AND user_credentials.password = '{password}';")
                .ToList();

            if (!query.Any())
            {
                MessageBox.Show("This user does not exist.");
            }
            else
            {
                var user = query.First();
                var newName = Microsoft.VisualBasic.Interaction.InputBox("Enter a new name:");
                user.Name = newName;
                _context.SaveChanges();
                MessageBox.Show("Name changed");
                if (_player1 is not null)
                {
                    if (user.Id == _player1.Id)
                    {
                        _player1 = user;
                        ChangeStatsForFirstPlayer();
                    }
                }

                if (_player2 is not null)
                {
                    if (user.Id == _player2.Id && _player2 is not null)
                    {
                        _player2 = user;
                        ChangeStatsForSecondPlayer();
                    }
                }
            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var id = Microsoft.VisualBasic.Interaction.InputBox("Enter a id:");
            var password = Microsoft.VisualBasic.Interaction.InputBox("Enter a password:");
            var query = _context.UserCredentials.FromSqlRaw(
                    $"SELECT id, password FROM user_credentials  WHERE user_credentials.id = {id} AND user_credentials.password = '{password}';")
                .ToList();

            if (!query.Any())
            {
                MessageBox.Show("This user does not exist.");
            }
            else
            {
                var userCredential = query.First();
                var newPassword = Microsoft.VisualBasic.Interaction.InputBox("Enter a new password:");
                userCredential.Password = newPassword;
                _context.SaveChanges();
                MessageBox.Show("Password changed");
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var id = Microsoft.VisualBasic.Interaction.InputBox("Enter a id:");
            var password = Microsoft.VisualBasic.Interaction.InputBox("Enter a password:");
            var query = _context.Users.FromSqlRaw(
                    $"SELECT u.id, u.name, u.win, u.lose, u.draw FROM users u INNER JOIN user_credentials on u.id = user_credentials.id WHERE user_credentials.id = {id} AND user_credentials.password = '{password}';")
                .ToList();

            if (!query.Any())
            {
                MessageBox.Show("This user does not exist.");
            }
            else
            {
                var user = query.First();
                var userCredential = _context.UserCredentials.Find(user.Id);

                if (_player1 is not null)
                {
                    if (user.Id == _player1.Id)
                    {
                        _player1 = null;
                        ChangeStatsForFirstPlayer();
                    }
                }

                if (_player2 is not null)
                {
                    if (user.Id == _player2.Id && _player2 is not null)
                    {
                        _player2 = null;
                        ChangeStatsForSecondPlayer();
                    }
                }

                _context.Remove(user);
                _context.Remove(userCredential);
                _context.SaveChanges();
                MessageBox.Show("User deleted");
            }
        }


        private void FindIdUser_Click(object sender, RoutedEventArgs e)
        {
            var name = Microsoft.VisualBasic.Interaction.InputBox("Enter a name:");
            var password = Microsoft.VisualBasic.Interaction.InputBox("Enter a password:");
            var query = _context.Users.FromSqlRaw(
                    $"SELECT u.id, u.name, u.win, u.lose, u.draw FROM users u INNER JOIN user_credentials on u.id = user_credentials.id WHERE u.name = '{name}' AND user_credentials.password = '{password}';")
                .ToList();

            MessageBox.Show(!query.Any() ? "This user does not exist." : $"ID = {query.First().Id}");
        }

        private void SetPlayer_Click(object sender, RoutedEventArgs e)
        {
            var tempPlayer = (sender as Button)!.Content.ToString()!.Contains('1') ? _player1 : _player2;


            if (tempPlayer != null)
            {
                if (MessageBox.Show("If you click yes, the information about the previous player will disappear", "",
                        MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    return;
                }
            }


            var id = Microsoft.VisualBasic.Interaction.InputBox("Enter a id:");
            var password = Microsoft.VisualBasic.Interaction.InputBox("Enter a password:");
            if (!int.TryParse(id, out _))
            {
                MessageBox.Show("Id is incorrect");
                return;
            }

            var query = _context.Users.FromSqlRaw(
                    $"SELECT u.id, u.name, u.win, u.lose, u.draw FROM users u INNER JOIN user_credentials on u.id = user_credentials.id WHERE user_credentials.id = {id} AND user_credentials.password = '{password}';")
                .ToList();

            if (!query.Any())
            {
                MessageBox.Show("This user does not exist.");
            }
            else
            {
                var user = query.First();
                if ((sender as Button)!.Content.ToString()!.Contains('1'))
                {
                    _player1 = user;
                    ChangeStatsForFirstPlayer();
                }
                else
                {
                    _player2 = user;
                    ChangeStatsForSecondPlayer();
                }

                MessageBox.Show("Hello, " + user.Name);
            }
        }


        private void ExitPlayer_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)!.Content.ToString()!.Contains('1'))
            {
                if (_player1 == null)
                {
                    return;
                }
                if (MessageBox.Show("You want exit?", "", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

                _player1 = null;
                ChangeStatsForFirstPlayer();
            }
            else
            {
                if (_player2 == null)
                {
                    return;
                }
                if (MessageBox.Show("You want exit?", "", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

                _player2 = null;
                ChangeStatsForSecondPlayer();
            }
        }

        private void ChangeStatsForFirstPlayer()
        {
            if (_player1 is null)
            {
                FirstPlayerId.Text = "Id: ";
                FirstPlayerName.Text = "Name: ";
                FirstPlayerWin.Text = "Win: ";
                FirstPlayerLose.Text = "Lose: ";
                FirstPlayerDraw.Text = "Draw: ";
            }
            else
            {
                FirstPlayerId.Text = "Id: " + _player1.Id;
                FirstPlayerName.Text = "Name: " + _player1.Name;
                FirstPlayerWin.Text = "Win: " + _player1.Win;
                FirstPlayerLose.Text = "Lose: " + _player1.Lose;
                FirstPlayerDraw.Text = "Draw: " + _player1.Draw;
            }
        }

        private void ChangeStatsForSecondPlayer()
        {
            if (_player2 is null)
            {
                SecondPlayerId.Text = "Id: ";
                SecondPlayerName.Text = "Name: ";
                SecondPlayerWin.Text = "Win: ";
                SecondPlayerLose.Text = "Lose: ";
                SecondPlayerDraw.Text = "Draw: ";
            }
            else
            {
                SecondPlayerId.Text = "Id: " + _player2.Id;
                SecondPlayerName.Text = "Name: " + _player2.Name;
                SecondPlayerWin.Text = "Win: " + _player2.Win;
                SecondPlayerLose.Text = "Lose: " + _player2.Lose;
                SecondPlayerDraw.Text = "Draw: " + _player2.Draw;
            }
        }


        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            if (_player1 is not null && _player2 is not null)
            {
                if (!_isNowGame)
                {
                    MessageBox.Show("The game is started");
                    ClearMainGame();
                    _isNowGame = true;
                    _playersMove = 0;
                    PlayerMoveLabel.Content = "The player's move: Player 1";
                }
                else
                {
                    MessageBox.Show("The game continues");
                }
            }
            else
            {
                MessageBox.Show("Not enough players");
            }
        }

        private void DefaultViewMainGame()
        {
            Button1.Content = "X";
            Button2.Content = "O";
            Button3.Content = "X";
            Button4.Content = "O";
            Button5.Content = "X";
            Button6.Content = "O";
            Button7.Content = "X";
            Button8.Content = "O";
            Button9.Content = "X";
        }

        private void ClearMainGame()
        {
            foreach (UIElement element in MainGame.Children)
            {
                if (element is Button button)
                {
                    button.Content = "";
                }
            }
        }

        private void EndGame_Click(object sender, RoutedEventArgs e)
        {
            if (_isNowGame)
            {
                if (MessageBox.Show("End the game?", "", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
                _isNowGame = false;
                DefaultViewMainGame();
                PlayerMoveLabel.Content = "The player's move:";
            }
            else
            {
                MessageBox.Show("The game has not started");
            }
        }

        private void Game_Click(object sender, RoutedEventArgs e)
        {
            if (!_isNowGame) return;
            sender.GetType().GetProperty("Content")?.SetValue(sender, _playersMove % 2 == 0 ? "X" : "O");
            var user1 = _context.Users.First(a => a.Id == _player1.Id);
            var user2 = _context.Users.First(a => a.Id == _player2.Id);
            if (CheckWin())
            {
                MessageBox.Show((_playersMove % 2 == 0 ? "Player 1" : "Player 2") + " win");
                _isNowGame = false;
                DefaultViewMainGame();
                PlayerMoveLabel.Content = "The player's move:";

                if (_playersMove % 2 == 0)
                {
                    user1.Win++;
                    user2.Lose++;
                }
                else
                {
                    user2.Win++;
                    user1.Lose++;
                }

                _context.SaveChanges();
                _player1 = user1;
                _player2 = user2;
                ChangeStatsForFirstPlayer();
                ChangeStatsForSecondPlayer();
                return;
            }

            _playersMove++;
            if (_playersMove == 9)
            {
                _isNowGame = false;
                DefaultViewMainGame();
                PlayerMoveLabel.Content = "The player's move:";
                MessageBox.Show("Draw!");
                user1.Draw++;
                user2.Draw++;
            }

            _context.SaveChanges();
            _player1 = user1;
            _player2 = user2;
            ChangeStatsForFirstPlayer();
            ChangeStatsForSecondPlayer();
            PlayerMoveLabel.Content = "The player's move: " + (_playersMove % 2 == 0 ? "Player 1" : "Player 2");
        }

        private bool CheckWin()
        {
            //rows
            if (Button1.Content == Button2.Content && Button2.Content == Button3.Content && Button1.Content != "")
            {
                return true;
            }

            if (Button4.Content == Button5.Content && Button5.Content == Button6.Content && Button4.Content != "")
            {
                return true;
            }

            if (Button7.Content == Button8.Content && Button8.Content == Button9.Content && Button7.Content != "")
            {
                return true;
            }
            //////////////////


            //columns
            if (Button1.Content == Button4.Content && Button4.Content == Button7.Content && Button1.Content != "")
            {
                return true;
            }

            if (Button2.Content == Button5.Content && Button5.Content == Button8.Content && Button2.Content != "")
            {
                return true;
            }

            if (Button3.Content == Button6.Content && Button6.Content == Button9.Content && Button3.Content != "")
            {
                return true;
            }
            ////////////////////


            //diagonal

            if (Button1.Content == Button5.Content && Button5.Content == Button9.Content && Button1.Content != "")
            {
                return true;
            }

            if (Button7.Content == Button5.Content && Button5.Content == Button3.Content && Button7.Content != "")
            {
                return true;
            }

            return false;
        }
    }
}