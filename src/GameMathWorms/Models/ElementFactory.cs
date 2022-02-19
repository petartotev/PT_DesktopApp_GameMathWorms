using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameMathWorms.Models
{
    public static class ElementFactory
    {
        public static Label CreateNewLabel(string name, string content, int fontSize, Thickness margin, Thickness padding = default, Action<Label> setup = null)
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

        internal static Rectangle CreateNewRectangle(string name, int width, int height, int opacity = 1, Action<Rectangle> setup = null)
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
