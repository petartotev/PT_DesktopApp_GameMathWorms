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
        private readonly Target _target;
        private readonly Player _playerBlue;
        private readonly Player _playerRed;

        private readonly Random _random = new Random();
        private readonly DispatcherTimer _gameTimer = new DispatcherTimer();
        private readonly DispatcherTimer _targetTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            _playerBlue = new Player(Blue);
            _playerRed = new Player(Red);
            _target = new Target(SomeLabel);

            PlayIntro();

            GameCanvas.Focus();
        }

        private void StartGame()
        {
            _gameTimer.Tick += GameTimerEvent;
            _gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            _gameTimer.Start();

            _targetTimer.Tick += TargetTimeEvent;
            _targetTimer.Interval = TimeSpan.FromSeconds(GameConstants.Target.ResetSeconds);
            _targetTimer.Start();
        }

        private void PlayIntro()
        {
            GameAnimator.AnimateOpacity(TextWelcome, isAutoReverse: true);
            GameAnimator.AnimateOpacity(TextTo, isAutoReverse: true);
            GameAnimator.AnimateOpacity(TextMathWorms, isAutoReverse: true);

            GameAnimator.AnimateOpacity(TextPressEnter, 5000, true);

            GameAnimator.AnimateOpacity(_playerBlue.Image);
            GameAnimator.AnimateOpacity(_playerRed.Image);
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            MovePlayerOnCanvas(_playerBlue);
            MovePlayerOnCanvas(_playerRed);
        }

        private void TargetTimeEvent(object sender, EventArgs e)
        {
            _target.SetRandomly();
            MoveTargetOnCanvas(_target);
        }

        private void MoveTargetOnCanvas(Target target)
        {
            Canvas.SetLeft(
                target.Label,
                _random.Next(GameConstants.Target.MinPositionX, GameConstants.Target.MaxPositionX));
            Canvas.SetTop(
                target.Label,
                _random.Next(GameConstants.Target.MinPositionY, GameConstants.Target.MaxPositionY));
        }

        private void MovePlayerOnCanvas(Player player)
        {
            if (player.IsMovingLeft && Canvas.GetLeft(player.Image) > 10)
            {
                Canvas.SetLeft(player.Image, Canvas.GetLeft(player.Image) - GameConstants.PlayerSpeed);
                ((ScaleTransform)player.Image.RenderTransform).ScaleX = -1;
            }
            if (player.IsMovingRight && Canvas.GetLeft(player.Image) + (player.Image.Width * 1.4) < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player.Image, Canvas.GetLeft(player.Image) + GameConstants.PlayerSpeed);
                ((ScaleTransform)player.Image.RenderTransform).ScaleX = 1;
            }
        }

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                StartGame();
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
