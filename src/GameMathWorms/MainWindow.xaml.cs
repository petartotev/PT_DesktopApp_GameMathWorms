using GameMathWorms.Constants;
using GameMathWorms.Models;
using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameMathWorms
{
    public partial class MainWindow : Window
    {
        private readonly Random _random = new Random();
        private readonly SoundPlayer _soundPlayer = new SoundPlayer(@$"../../../../../res/audio/clockmaker.wav");

        private Target _target;
        private Player _playerBlue;
        private Player _playerRed;
        private Canvas _canvas;

        private Button _buttonPlay;
        private Button _buttonExit;
        private Button _buttonMusic;

        private bool _isMusicOn = false;

        private DispatcherTimer _gameTimer = new DispatcherTimer();
        private DispatcherTimer _targetTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            _soundPlayer.PlayLooping();

            InitializeGame();
        }

        // ==================== Game Flow ====================
        private void InitializeGame()
        {
            _ = AddPlayerImageElement("ImageBlue", "/worm_cyan.png", scaleX: 1, new Thickness(-720, 340, 0, 0));
            _ = AddPlayerImageElement("ImageRed", "/worm_magenta.png", scaleX: -1, new Thickness(720, 340, 0, 0));

            Label labelWelcome = ElementFactory.CreateLabel("LabelWelcome", "WELCOME", 20, new Thickness(115, 305, 0, 0));
            _ = GameGrid.Children.Add(labelWelcome);
            AnimateOpacity(labelWelcome, 0, 1, 2000, actionOnEnd: x => GameGrid.Children.Remove(labelWelcome));

            Label labelTo = ElementFactory.CreateLabel("LabelTo", "TO", 20, new Thickness(115, 330, 0, 0));
            _ = GameGrid.Children.Add(labelTo);
            AnimateOpacity(labelTo, 0, 1, 2500, actionOnEnd: x => GameGrid.Children.Remove(labelTo));

            Label labelMathWorms = ElementFactory.CreateLabel("LabelMathWorms", "MATH WORMS!", 70, new Thickness(115, 345, 0, 0));
            _ = GameGrid.Children.Add(labelMathWorms);
            AnimateOpacity(labelMathWorms, 0, 1, 3000, actionOnEnd: x => AddMenuButtons());
        }

        private void StartGame()
        {
            if (!_isMusicOn)
            {
                _soundPlayer.Stop();
            }

            RemoveMenuButtons();

            GameGrid.Children.Clear();

            _canvas = ElementFactory.CreateCanvas("GameCanvas");
            _canvas.KeyDown += new KeyEventHandler(HandleKeyDownEvent);
            _canvas.KeyUp += new KeyEventHandler(HandleKeyUpEvent);
            _ = GameGrid.Children.Add(_canvas);

            AddMusicButton();

            Label labelBlueScore = ElementFactory.CreateLabel("LabelBlueScore", "", 30, new Thickness(0, 0, 0, 0),
                setup: x => x.Foreground = new SolidColorBrush(Colors.DarkCyan));
            Canvas.SetLeft(labelBlueScore, 175);
            Canvas.SetTop(labelBlueScore, 0);
            _ = _canvas.Children.Add(labelBlueScore);

            Label labelRedScore = ElementFactory.CreateLabel("LabelRedScore", "", 30, new Thickness(0, 0, 0, 0),
                setup: x => x.Foreground = new SolidColorBrush(Colors.Magenta));
            Canvas.SetLeft(labelRedScore, 600);
            Canvas.SetTop(labelRedScore, 0);
            _ = _canvas.Children.Add(labelRedScore);

            Image imageBlue = ElementFactory.CreateImage("ImageBlue", 60, 70, "/worm_cyan.png");
            _ = _canvas.Children.Add(imageBlue);
            Canvas.SetLeft(imageBlue, 0);
            Canvas.SetTop(imageBlue, 340);
            imageBlue.RenderTransform = new ScaleTransform { ScaleX = 1 };

            Image imageRed = ElementFactory.CreateImage("ImageBlue", 60, 70, "/worm_magenta.png");
            _ = _canvas.Children.Add(imageRed);
            Canvas.SetLeft(imageRed, 720);
            Canvas.SetTop(imageRed, 340);
            imageRed.RenderTransform = new ScaleTransform { ScaleX = -1 };

            Label labelTarget = ElementFactory.CreateLabel("LabelTarget", "+-0", 21, new Thickness(0, 0, 0, 0),
            setup: x =>
            {
                x.Background = new SolidColorBrush(Colors.Black);
                x.Foreground = new SolidColorBrush(Colors.White);
            });
            Canvas.SetLeft(labelTarget, 372);
            Canvas.SetTop(labelTarget, -32);
            _ = _canvas.Children.Add(labelTarget);

            _playerBlue = new Player(imageBlue, labelBlueScore);
            _playerRed = new Player(imageRed, labelRedScore);
            _target = new Target(labelTarget);

            _ = _canvas.Focus();

            SetFinalGoalValue();

            _gameTimer.Stop();
            _gameTimer = new DispatcherTimer();
            _gameTimer.Tick += PlayerMovementEvent;
            _gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            _gameTimer.Start();

            _targetTimer.Stop();
            _targetTimer = new DispatcherTimer();
            _targetTimer.Tick += TargetMovementEvent;
            _targetTimer.Interval = TimeSpan.FromSeconds(GameConstants.Target.ResetSeconds);
            _targetTimer.Start();
        }

        private void ExitGame()
        {
            RemoveMenuButtons();

            Rectangle rectangleBye = ElementFactory.CreateRectangle(nameof(rectangleBye), 800, 450, 1,
                setup: x => x.Fill = new SolidColorBrush(Colors.Black));
            Grid.SetZIndex(rectangleBye, 98);
            _ = GameGrid.Children.Add(rectangleBye);
            AnimateOpacity(rectangleBye, 0, 1, 1000);

            Label labelBye = ElementFactory.CreateLabel(nameof(labelBye), "BYE...", 70, new Thickness(300, 175, 0, 0),
                setup: x => x.Foreground = new SolidColorBrush(Colors.White));
            Grid.SetZIndex(labelBye, 99);
            _ = GameGrid.Children.Add(labelBye);
            AnimateOpacity(labelBye, 0, 1, 3000, actionOnEnd: x => Application.Current.Shutdown());
        }

        private void SetFinalGoalValue()
        {
            Label labelFinalGoalText = ElementFactory.CreateLabel("LabelFinalGoalText", "FINAL GOAL", 10, new Thickness(355, 0, 0, 0));
            _ = _canvas.Children.Add(labelFinalGoalText);

            Label labelFinalGoalValue = ElementFactory.CreateLabel("LabelFinalGoalValue", "0", 30, new Thickness(360, 15, 0, 0));
            _ = _canvas.Children.Add(labelFinalGoalValue);
            labelFinalGoalValue.Content = GameConstants.Game.FinalGoal;

            _playerBlue.ScoreText.Content = _playerBlue.Score;
            _playerRed.ScoreText.Content = _playerRed.Score;
        }

        private void MovePlayerOnCanvas(Player player)
        {
            if (player.IsMovingLeft && Canvas.GetLeft(player.Image) > 10)
            {
                Canvas.SetLeft(player.Image, Canvas.GetLeft(player.Image) - GameConstants.Player.Speed);
                ((ScaleTransform)player.Image.RenderTransform).ScaleX = -1;
            }
            else if (player.IsMovingRight && Canvas.GetLeft(player.Image) + (player.Image.Width * 1.4) < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player.Image, Canvas.GetLeft(player.Image) + GameConstants.Player.Speed);
                ((ScaleTransform)player.Image.RenderTransform).ScaleX = 1;
            }
        }

        private void MoveTargetOnCanvas()
        {
            _target.RandomlySet();

            Canvas.SetLeft(_target.Label, _random.Next(GameConstants.Target.MinPositionX, GameConstants.Target.MaxPositionX));
            Canvas.SetTop(_target.Label, GameConstants.Target.MinPositionY);

            AnimateFallingObject(_target);
        }

        private void UpdatePlayerScoreIfIntersectsWithTarget(Player player)
        {
            if (Canvas.GetLeft(_target.Label) > Canvas.GetLeft(player.Image) - 100 &&
                Canvas.GetLeft(_target.Label) < Canvas.GetLeft(player.Image) + 100 &&
                Canvas.GetTop(_target.Label) >= Canvas.GetTop(player.Image))
            {
                player.RecalculateScore(_target);
                player.ScoreText.Content = player.Score;
            }
        }

        private bool IsPlayerWinner(Player player)
        {
            return player.Score == GameConstants.Game.FinalGoal;
        }

        private void PlayWonAnimation(Player player)
        {
            Canvas.SetZIndex(player.Image, 2);

            Rectangle rectWon = ElementFactory.CreateRectangle(nameof(rectWon), 800, 450, 1,
                setup: x => x.Fill = new SolidColorBrush(Colors.GreenYellow));
            Canvas.SetZIndex(rectWon, 1);
            _ = _canvas.Children.Add(rectWon);

            Label labelWon = ElementFactory.CreateLabel(nameof(labelWon), $"WON!", 70, new Thickness(300, 50, 0, 0),
                setup: x => x.Foreground = new SolidColorBrush(Colors.Black));
            Canvas.SetZIndex(labelWon, 2);
            _ = _canvas.Children.Add(labelWon);

            AnimateOpacity(rectWon, 0, 1, 1000);
            AnimateOpacity(labelWon, 0, 1, 1000, actionOnEnd: x => AddMenuButtons());
        }

        // ==================== Elements ====================
        private Image AddPlayerImageElement(string name, string url, int scaleX, Thickness margin = default)
        {
            Image imagePlayer = ElementFactory.CreateImage(name, 60, 70, url, x => x.Margin = margin);
            _ = GameGrid.Children.Add(imagePlayer);
            imagePlayer.RenderTransform = new ScaleTransform { ScaleX = scaleX };
            AnimateOpacity(imagePlayer, 0, 1);

            return imagePlayer;
        }

        // ==================== Buttons ====================
        private void AddMenuButtons()
        {
            _buttonPlay = ElementFactory.CreateButton(180, 45, "PLAY", new Thickness(0, -50, 0, 0),
                setup: x => x.Click += ClickOnButtonPlay);
            _ = GameGrid.Children.Add(_buttonPlay);

            _buttonExit = ElementFactory.CreateButton(180, 45, "EXIT", new Thickness(0, 50, 0, 0),
                setup: x => x.Click += ClickOnButtonExit);
            _ = GameGrid.Children.Add(_buttonExit);
        }

        private void AddMusicButton()
        {
            _buttonMusic = ElementFactory.CreateButton(30, 30, "OFF", new Thickness(-0, -0, 0, 0),
                setup: x =>
                {
                    x.Content = _isMusicOn ? "ON" : "OFF";
                    x.Click += ClickOnButtonMusic;
                    x.FontSize = 10;
                });
            Canvas.SetLeft(_buttonMusic, 5);
            Canvas.SetTop(_buttonMusic, 5);
            _ = _canvas.Children.Add(_buttonMusic);
        }

        private void RemoveMenuButtons()
        {
            GameGrid.Children.Remove(_buttonPlay);
            GameGrid.Children.Remove(_buttonExit);
        }

        // ==================== Button Events ====================
        private void ClickOnButtonPlay(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void ClickOnButtonExit(object sender, RoutedEventArgs e)
        {
            ExitGame();
        }

        private void ClickOnButtonMusic(object sender, RoutedEventArgs e)
        {
            _isMusicOn = !_isMusicOn;

            if (_isMusicOn)
            {
                _buttonMusic.Content = "ON";
                _buttonMusic.Background = new SolidColorBrush(Colors.Black);
                _buttonMusic.Foreground = new SolidColorBrush(Colors.White);
                _soundPlayer.PlayLooping();
            }
            else
            {
                _buttonMusic.Content = "OFF";
                _buttonMusic.Background = new SolidColorBrush(Colors.White);
                _buttonMusic.Foreground = new SolidColorBrush(Colors.Black);
                _soundPlayer.Stop();
            }
        }

        // ==================== Timer Events ====================
        private void PlayerMovementEvent(object sender, EventArgs e)
        {
            MovePlayerOnCanvas(_playerBlue);
            MovePlayerOnCanvas(_playerRed);
        }

        private void TargetMovementEvent(object sender, EventArgs e)
        {
            MoveTargetOnCanvas();
        }

        // ==================== Key Down Events ====================
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

        // ==================== Animations ====================
        private void AnimateOpacity(
            FrameworkElement element,
            int from,
            int to,
            int duration = 3000,
            Action<FrameworkElement> actionOnEnd = null)
        {
            DoubleAnimation animationOpacity = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration))
            };

            Storyboard.SetTarget(animationOpacity, element);
            Storyboard.SetTargetProperty(animationOpacity, new PropertyPath(Label.OpacityProperty));

            Storyboard storyboardOpacity = new Storyboard();
            storyboardOpacity.Children.Add(animationOpacity);
            storyboardOpacity.Completed += (o, c) =>
            {
                actionOnEnd?.Invoke(element);
            };
            storyboardOpacity.Begin(element);
        }

        private void AnimateFallingObject(Target target, int targetSpeed = GameConstants.Target.SpeedFalling)
        {
            DoubleAnimation animationFalling = new DoubleAnimation
            {
                From = GameConstants.Target.MinPositionY,
                To = GameConstants.Canvas.HeightMax + 200,
                Duration = new Duration(TimeSpan.FromSeconds(targetSpeed)),
                AutoReverse = false
            };

            Storyboard.SetTarget(animationFalling, target.Label);
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
