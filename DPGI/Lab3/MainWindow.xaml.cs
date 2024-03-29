using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Player _player1 = null;
        private Player _player2 = null;
        private bool _isNowGame;
        private int _playersMove;


        public MainWindow()
        {
            InitializeComponent();
            _isNowGame = false;
            _playersMove = 0;
        }

        private void FrameworkElement_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (UIElement element in TextBlockPanel1.Children)
            {
                if (element is TextBlock textBlock)
                {
                    textBlock.FontSize = 22 * e.NewSize.Height / 500;
                }
            }

            foreach (UIElement element in TextBlockPanel2.Children)
            {
                if (element is TextBlock textBlock)
                {
                    textBlock.FontSize = 22 * e.NewSize.Height / 500;
                }
            }

            foreach (UIElement element in MainGame.Children)
            {
                if (element is Button button)
                {
                    button.FontSize = 68 * e.NewSize.Height / 500;
                }
            }
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


            string name;
            do
            {
                name = Microsoft.VisualBasic.Interaction.InputBox("Enter a name of up to 10 characters:");
            } while (name.Length is > 10 or 0);

            if ((sender as Button)!.Content.ToString()!.Contains('1'))
            {
                _player1 = new Player(name);
                ChangeStatsForFirstPlayer();
            }
            else
            {
                _player2 = new Player(name);
                ChangeStatsForSecondPlayer();
            }
        }

        private void ChangeStatsForFirstPlayer()
        {
            FirstPlayerName.Text = "Name: " + _player1.Name;
            FirstPlayerWin.Text = "Win: " + _player1.Win;
            FirstPlayerLose.Text = "Lose: " + _player1.Lose;
            FirstPlayerDraw.Text = "Draw: " + _player1.Draw;
        }

        private void ChangeStatsForSecondPlayer()
        {
            SecondPlayerName.Text = "Name: " + _player2.Name;
            SecondPlayerWin.Text = "Win: " + _player2.Win;
            SecondPlayerLose.Text = "Lose: " + _player2.Lose;
            SecondPlayerDraw.Text = "Draw: " + _player2.Draw;
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

            if (CheckWin())
            {
                MessageBox.Show((_playersMove % 2 == 0 ? "Player 1" : "Player 2") + " win");
                _isNowGame = false;
                DefaultViewMainGame();
                PlayerMoveLabel.Content = "The player's move:";

                if (_playersMove % 2 == 0)
                {
                    _player1.WinGame();
                    _player2.LoseGame();
                }
                else
                {
                    _player2.WinGame();
                    _player1.LoseGame();
                }

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
                _player1.DrawGame();
                _player2.DrawGame();
                ChangeStatsForFirstPlayer();
                ChangeStatsForSecondPlayer();
            }

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