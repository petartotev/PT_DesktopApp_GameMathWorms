using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GameMathWorms.Models
{
    // https://docs.microsoft.com/en-us/dotnet/desktop/wpf/graphics-multimedia/how-to-animate-a-property-by-using-a-storyboard?view=netframeworkdesktop-4.8
    public static class GameAnimator
    {
        public static void AnimateOpacity(FrameworkElement element, int duration = 3000, bool isAutoReverse = false)
        {
            var animation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                AutoReverse = isAutoReverse
            };

            Storyboard.SetTargetName(animation, element.Name);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Label.OpacityProperty));

            // Create a storyboard to contain the animation.
            Storyboard myWidthAnimatedButtonStoryboard = new Storyboard();
            myWidthAnimatedButtonStoryboard.Children.Add(animation);
            myWidthAnimatedButtonStoryboard.Begin(element);
        }
    }
}
