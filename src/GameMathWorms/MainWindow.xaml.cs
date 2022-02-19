using GameMathWorms.Constants;
using GameMathWorms.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameMathWorms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Target _target;
        private readonly Player _playerBlue;
        private readonly Player _playerRed;

        private readonly Random _random = new Random();
        private readonly DispatcherTimer _gameTimer = new DispatcherTimer();
        private readonly DispatcherTimer _targetTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            _playerBlue = new Player(ImageBlue, LabelBlueScore);
            _playerRed = new Player(ImageRed, LabelRedScore);
            _target = new Target(LabelTarget);

            InitializeGame();

            GameCanvas.Focus();
        }

        private void InitializeGame()
        {
            PlayIntro();
        }

        // ========== Timer Events ==========
        private void GameTimerEvent(object sender, EventArgs e)
        {
            MovePlayerOnCanvas(_playerBlue);
            MovePlayerOnCanvas(_playerRed);
        }

        private void TargetTimeEvent(object sender, EventArgs e)
        {
            MoveTargetOnCanvas();
        }

        // ========== Key Down Events ==========
        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                StartGame();
            }
            if (e.Key == Key.Escape)
            {
                ExitGame();
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

        // ========== Game Flow ==========
        private void PlayIntro()
        {
            Label labelWelcome = ElementFactory.CreateNewLabel("LabelWelcome", "WELCOME", 20, new Thickness(115, 305, 0, 0));
            _ = GameCanvas.Children.Add(labelWelcome);
            AnimateOpacity(labelWelcome, 0, 1, 5000, isAutoReverse: false,
                actionOnEnd: x => GameCanvas.Children.Remove(labelWelcome));

            Label labelTo = ElementFactory.CreateNewLabel("LabelTo", "TO", 20, new Thickness(115, 330, 0, 0));
            _ = GameCanvas.Children.Add(labelTo);
            AnimateOpacity(labelTo, 0, 1, 5000, isAutoReverse: false,
                actionOnEnd: x => GameCanvas.Children.Remove(labelTo));

            Label labelMathWorms = ElementFactory.CreateNewLabel("LabelMathWorms", "MATH WORMS!", 70, new Thickness(115, 345, 0, 0));
            _ = GameCanvas.Children.Add(labelMathWorms);
            AnimateOpacity(labelMathWorms, 0, 1, 5000, isAutoReverse: false,
                actionOnEnd: x => GameCanvas.Children.Remove(labelMathWorms));

            Label labelPressEnter = ElementFactory.CreateNewLabel("LabelPressEnter", "PRESS ENTER TO PROCEED...", 10, new Thickness(0, 200, 0, 0));
            _ = GameCanvas.Children.Add(labelPressEnter);
            AnimateOpacity(labelPressEnter, 0, 1, 5000, true);

            AnimateOpacity(_playerBlue.Image, 0, 1);
            AnimateOpacity(_playerRed.Image, 0, 1);
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

        private void SetFinalGoalValue()
        {
            Label labelFinalGoalText = ElementFactory.CreateNewLabel("LabelFinalGoalText", "FINAL GOAL", 10, new Thickness(355, 0, 0, 0));
            _ = GameCanvas.Children.Add(labelFinalGoalText);

            Label labelFinalGoalValue = ElementFactory.CreateNewLabel("LabelFinalGoalValue", "0", 30, new Thickness(360, 15, 0, 0));
            _ = GameCanvas.Children.Add(labelFinalGoalValue);
            labelFinalGoalValue.Content = GameConstants.Game.FinalGoal;

            LabelBlueScore.Content = _playerBlue.Score;
            LabelRedScore.Content = _playerRed.Score;
        }

        private void ExitGame()
        {
            Rectangle rectBye = ElementFactory.CreateNewRectangle(nameof(rectBye), 800, 450, 1,
                setup: x => x.Fill = new SolidColorBrush(Colors.Black));
            Canvas.SetZIndex(rectBye, 98);
            _ = GameCanvas.Children.Add(rectBye);

            Label labelBye = ElementFactory.CreateNewLabel(nameof(labelBye), "BYE...", 70, new Thickness(300, 175, 0, 0),
                setup: x => x.Foreground = new SolidColorBrush(Colors.White));
            Canvas.SetZIndex(labelBye, 99);
            _ = GameCanvas.Children.Add(labelBye);

            AnimateOpacity(rectBye, 0, 1, 3000, isAutoReverse: false);
            AnimateOpacity(labelBye, 0, 1, 6000, isAutoReverse: false, actionOnEnd: x => Application.Current.Shutdown());
        }

        private void PlayWonAnimation(Player player)
        {
            Rectangle rectWon = ElementFactory.CreateNewRectangle(nameof(rectWon), 800, 450, 1,
                setup: x => x.Fill = new SolidColorBrush(Colors.GreenYellow));
            Canvas.SetZIndex(rectWon, 1);
            _ = GameCanvas.Children.Add(rectWon);

            Canvas.SetZIndex(player.Image, 2);

            Label labelWon = ElementFactory.CreateNewLabel(nameof(labelWon), $"WON!", 70, new Thickness(300, 175, 0, 0),
                setup: x => x.Foreground = new SolidColorBrush(Colors.Black));
            Canvas.SetZIndex(labelWon, 2);
            _ = GameCanvas.Children.Add(labelWon);
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

        private void MoveTargetOnCanvas()
        {
            _target.SetRandomly();

            Canvas.SetLeft(_target.Label, _random.Next(GameConstants.Target.MinPositionX, GameConstants.Target.MaxPositionX));
            Canvas.SetTop(_target.Label, GameConstants.Target.MinPositionY);

            AnimateFallingObject(_target);
        }

        private void UpdatePlayerScoreIfIntersectsWithTarget(Player player)
        {
            DisplayCurrentPositions();

            if (Canvas.GetLeft(_target.Label) > Canvas.GetLeft(player.Image) - 100 &&
                Canvas.GetLeft(_target.Label) < Canvas.GetLeft(player.Image) + 100 &&
                Canvas.GetTop(_target.Label) >= Canvas.GetTop(player.Image))
            {
                player.RecalculateScore(_target);
                player.ScoreText.Content = player.Score;
            }
        }

        private void DisplayCurrentPositions()
        {
            double targetleft = Canvas.GetLeft(_target.Label);
            LabelTargetX.Content = targetleft;
            double targetTop = Canvas.GetTop(_target.Label);
            LabelTargetY.Content = targetTop;

            double playerRedLeft = Canvas.GetLeft(_playerRed.Image);
            LabelRedX.Content = playerRedLeft;
            double playerRedTop = Canvas.GetTop(_playerRed.Image);
            LabelRedY.Content = playerRedTop;

            double playerBlueLeft = Canvas.GetLeft(_playerBlue.Image);
            LabelBlueX.Content = playerBlueLeft;
            double playerBlueTop = Canvas.GetTop(_playerBlue.Image);
            LabelBlueY.Content = playerBlueTop;
        }

        private bool IsPlayerWinner(Player player)
        {
            return player.Score == GameConstants.Game.FinalGoal;
        }

        public void AnimateOpacity(
            FrameworkElement element,
            int from,
            int to,
            int duration = 3000,
            bool isAutoReverse = false,
            Action<FrameworkElement> actionOnEnd = null)
        {
            var animationOpacity = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                AutoReverse = isAutoReverse
            };

            Storyboard.SetTarget(animationOpacity, element);
            Storyboard.SetTargetProperty(animationOpacity, new PropertyPath(Label.OpacityProperty));

            // Create a storyboard to contain the animation.
            Storyboard storyboardOpacity = new Storyboard();
            storyboardOpacity.Children.Add(animationOpacity);
            storyboardOpacity.Completed += (o, c) =>
            {
                actionOnEnd?.Invoke(element);
            };
            storyboardOpacity.Begin(element);
        }

        // https://docs.microsoft.com/en-us/dotnet/desktop/wpf/graphics-multimedia/how-to-animate-a-property-by-using-a-storyboard?view=netframeworkdesktop-4.8
        public void AnimateFallingObject(
            Target target,
            int targetSpeed = GameConstants.Target.SpeedFalling)
        {
            DoubleAnimation animationFalling = new DoubleAnimation
            {
                From = GameConstants.Target.MinPositionY,
                To = GameConstants.Canvas.HeightMax + 200,
                Duration = new Duration(TimeSpan.FromSeconds(targetSpeed)),
                AutoReverse = false
            };

            Storyboard.SetTargetName(animationFalling, target.Label.Name);
            Storyboard.SetTargetProperty(animationFalling, new PropertyPath(Canvas.TopProperty));

            Storyboard storyboardFalling = new Storyboard();
            storyboardFalling.Children.Add(animationFalling);
            storyboardFalling.Completed += (o, c) =>
            {
                UpdatePlayerScoreIfIntersectsWithTarget(_playerBlue);
                UpdatePlayerScoreIfIntersectsWithTarget(_playerRed);

                if (IsPlayerWinner(_playerBlue))
                {
                    PlayWonAnimation(_playerBlue);
                }
                if (IsPlayerWinner(_playerRed))
                {
                    PlayWonAnimation(_playerRed);
                }
            };
            storyboardFalling.Begin(target.Label);
        }
    }
}
