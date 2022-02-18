using GameMathWorms.Constants;
using GameMathWorms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameMathWorms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _gameFinalGoal = 0;

        private readonly Target _target;
        private readonly Player _playerBlue;
        private readonly Player _playerRed;

        private readonly Random _random = new Random();
        private readonly DispatcherTimer _gameTimer = new DispatcherTimer();
        private readonly DispatcherTimer _targetTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            _playerBlue = new Player(Blue, TextBlueScore);
            _playerRed = new Player(Red, TextRedScore);
            _target = new Target(LabelTarget);

            PlayIntro();

            GameCanvas.Focus();
        }

        private void StartGame()
        {
            SetFinalGoalValue();

            _gameTimer.Tick += GameTimerEvent;
            _gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            _gameTimer.Start();

            _targetTimer.Tick += TargetTimeEvent;
            _targetTimer.Interval = TimeSpan.FromSeconds(GameConstants.Target.ResetSeconds);
            _targetTimer.Start();
        }

        private void EndGame()
        {
            GameAnimator.AnimateOpacity(TextBye, 0, 1, 6000, isAutoReverse: false);
            GameAnimator.AnimateOpacity(RectBye, 0, 1, 3000, isAutoReverse: false);
        }

        private void PlayIntro()
        {
            GameAnimator.AnimateOpacity(TextWelcome, 0, 1, isAutoReverse: true);
            GameAnimator.AnimateOpacity(TextTo, 0, 1, isAutoReverse: true);
            GameAnimator.AnimateOpacity(TextMathWorms, 0, 1, isAutoReverse: true);

            GameAnimator.AnimateOpacity(TextPressEnter, 0, 1, 5000, true);

            GameAnimator.AnimateOpacity(_playerBlue.Image, 0, 1);
            GameAnimator.AnimateOpacity(_playerRed.Image, 0, 1);
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            MovePlayerOnCanvas(_playerBlue);
            MovePlayerOnCanvas(_playerRed);
        }

        private void TargetTimeEvent(object sender, EventArgs e)
        {
            MoveTargetOnCanvas();
        }

        private void SetFinalGoalValue()
        {
            _gameFinalGoal = GameConstants.Game.FinalGoalMaxLimit;
            GameFinalGoal.Content = _gameFinalGoal;
        }

        private void MoveTargetOnCanvas()
        {
            _target.SetRandomly();

            Canvas.SetLeft(_target.Label, _random.Next(GameConstants.Target.MinPositionX, GameConstants.Target.MaxPositionX));
            Canvas.SetTop(_target.Label, GameConstants.Target.MinPositionY);

            GameAnimator.AnimateFallingObject(_target);

            //var targetleft = (Canvas.GetLeft(_target.Label));
            //TargetX.Content = (Canvas.GetLeft(_target.Label));
            //var targetTop = (Canvas.GetTop(_target.Label));
            //TargetY.Content = (Canvas.GetTop(_target.Label));

            //var playerRedLeft = (Canvas.GetLeft(_playerRed.Image));
            //RedX.Content = (Canvas.GetLeft(_playerRed.Image));
            //var playerRedTop = (Canvas.GetTop(_playerRed.Image));
            //RedY.Content = (Canvas.GetTop(_playerRed.Image));

            //var playerBlueLeft = (Canvas.GetLeft(_playerBlue.Image));
            //BlueX.Content = (Canvas.GetLeft(_playerBlue.Image));
            //var playerBlueTop = (Canvas.GetTop(_playerBlue.Image));
            //BlueY.Content = (Canvas.GetTop(_playerBlue.Image));

            CheckIfPlayerAndTargetIntersect(_playerBlue);
            CheckIfPlayerAndTargetIntersect(_playerRed);
        }

        private void CheckIfPlayerAndTargetIntersect(Player player)
        {
            if (Canvas.GetLeft(_target.Label) > Canvas.GetLeft(player.Image) &&
                Canvas.GetLeft(_target.Label) < Canvas.GetLeft(player.Image) + 300 &&
                Canvas.GetTop(_target.Label) >= Canvas.GetTop(player.Image))
            {
                player.RecalculateScore(_target);
                player.ScoreText.Content = player.Score;
            }
        }

        private void MovePlayerOnCanvas(Player player)
        {
            if (player.IsMovingLeft && Canvas.GetLeft(player.Image) > 10)
            {
                Canvas.SetLeft(player.Image, Canvas.GetLeft(player.Image) - GameConstants.Player.Speed);
                ((ScaleTransform)player.Image.RenderTransform).ScaleX = -1;
            }
            if (player.IsMovingRight && Canvas.GetLeft(player.Image) + (player.Image.Width * 1.4) < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player.Image, Canvas.GetLeft(player.Image) + GameConstants.Player.Speed);
                ((ScaleTransform)player.Image.RenderTransform).ScaleX = 1;
            }
        }

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                StartGame();
            }
            if (e.Key == Key.Escape)
            {
                EndGame();
            }
            if (e.Key == Key.A)
            {
                _playerBlue.IsMovingLeft = true;
            }
            if (e.Key == Key.D)
            {
                _playerBlue.IsMovingRight = true;
            }
            if (e.Key == Key.Left)
            {
                _playerRed.IsMovingLeft = true;
            }
            if (e.Key == Key.Right)
            {
                _playerRed.IsMovingRight = true;
            }
        }

        private void HandleKeyUpEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                _playerBlue.IsMovingLeft = false;
            }
            if (e.Key == Key.D)
            {
                _playerBlue.IsMovingRight = false;
            }
            if (e.Key == Key.Left)
            {
                _playerRed.IsMovingLeft = false;
            }
            if (e.Key == Key.Right)
            {
                _playerRed.IsMovingRight = false;
            }
        }

        // ----- Tempory Methods Stored As Examples -----
        private void CreateNewLabelOnGrid()
        {
            var label = new Label
            {
                Height = 28,
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Content = "Test1",
                Margin = new Thickness(211, 211, 0, 0)
            };

            GameGrid.Children.Add(label);
        }
    }
}
