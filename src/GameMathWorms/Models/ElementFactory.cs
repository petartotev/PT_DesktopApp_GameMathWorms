using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameMathWorms.Models
{
    internal static class ElementFactory
    {
        internal static Button CreateButton(int width, int height, string content, Thickness margin, Action<Button> setup = null)
        {
            Button button = new Button
            {
                Width = width,
                Height = height,
                Content = content,
                Background = new SolidColorBrush(Colors.Black),
                Foreground = new SolidColorBrush(Colors.White),
                FontFamily = new FontFamily("Arial Black"),
                Margin = margin
            };

            setup?.Invoke(button);

            return button;
        }

        internal static Canvas CreateCanvas(string name, Action<Canvas> setup = null)
        {
            Canvas canvas = new Canvas
            {
                Name = name,
                Background = new SolidColorBrush(Colors.Yellow),
                Focusable = true
            };

            setup?.Invoke(canvas);

            return canvas;
        }

        internal static Image CreateImage(string name, int width, int height, string url, Action<Image> setup = null)
        {
            BitmapImage imageBitmap = new BitmapImage();

            imageBitmap.BeginInit();
            imageBitmap.UriSource = new Uri(@$"../../..{url}", UriKind.Relative);
            imageBitmap.EndInit();

            Image image = new Image
            {
                Name = name,
                Width = width,
                Height = height,
                RenderTransformOrigin = new Point(0.5, 1),
                Source = imageBitmap
            };

            setup?.Invoke(image);

            return image;
        }

        internal static Label CreateLabel(string name, string content, int fontSize, Thickness margin, Thickness padding = default, Action<Label> setup = null)
        {
            Label label = new Label
            {
                Name = name,
                Content = content,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                FontWeight = FontWeights.Bold,
                FontStyle = FontStyles.Normal,
                FontSize = fontSize,
                Foreground = new SolidColorBrush(Colors.Black),
                FontFamily = new FontFamily("Arial Black"),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Top,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = padding,
                Margin = margin
            };

            setup?.Invoke(label);

            return label;
        }

        internal static Rectangle CreateRectangle(string name, int width, int height, int opacity = 1, Action<Rectangle> setup = null)
        {
            Rectangle rectangle = new Rectangle
            {
                Name = name,
                Fill = new SolidColorBrush(Colors.Black),
                Width = width,
                Height = height,
                Opacity = opacity,
            };

            setup?.Invoke(rectangle);

            return rectangle;
        }
    }
}