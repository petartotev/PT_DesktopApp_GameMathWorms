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
using System.Windows.Threading;

namespace GameMathWorms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool redGoLeft, redGoRight, blueGoLeft, blueGoRight;
        int playerSpeed = 10;
        int speed = 12;

        DispatcherTimer gameTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            GameCanvas.Focus();

            gameTimer.Tick += GameTimerEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            if (blueGoLeft && Canvas.GetLeft(Blue) > 10)
            {
                Canvas.SetLeft(Blue, Canvas.GetLeft(Blue) - playerSpeed);
                ((ScaleTransform)Blue.RenderTransform).ScaleX = -1;
            }
            if (blueGoRight && Canvas.GetLeft(Blue) + (Blue.Width * 1.4) < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(Blue, Canvas.GetLeft(Blue) + playerSpeed);
                ((ScaleTransform)Blue.RenderTransform).ScaleX = 1;
            }

            if (redGoLeft && Canvas.GetLeft(Red) > 10)
            {
                Canvas.SetLeft(Red, Canvas.GetLeft(Red) - playerSpeed);
                ((ScaleTransform)Red.RenderTransform).ScaleX = -1;
            }
            if (redGoRight && Canvas.GetLeft(Red) + (Red.Width * 1.4) < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(Red, Canvas.GetLeft(Red) + playerSpeed);
                ((ScaleTransform)Red.RenderTransform).ScaleX = 1;
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                blueGoLeft = true;
            }
            if (e.Key == Key.D)
            {
                blueGoRight = true;
            }
            if (e.Key == Key.Left)
            {
                redGoLeft = true;
            }
            if (e.Key == Key.Right)
            {
                redGoRight = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                blueGoLeft = false;
            }
            if (e.Key == Key.D)
            {
                blueGoRight = false;
            }
            if (e.Key == Key.Left)
            {
                redGoLeft = false;
            }
            if (e.Key == Key.Right)
            {
                redGoRight = false;
            }
        }
    }
}
