using GameMathWorms.Constants;
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
        public static void AnimateOpacity(FrameworkElement element, int from, int to, int duration = 3000, bool isAutoReverse = false)
        {
            var animationOpacity = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                AutoReverse = isAutoReverse
            };

            Storyboard.SetTargetName(animationOpacity, element.Name);
            Storyboard.SetTargetProperty(animationOpacity, new PropertyPath(Label.OpacityProperty));

            // Create a storyboard to contain the animation.
            Storyboard storyboardOpacity = new Storyboard();
            storyboardOpacity.Children.Add(animationOpacity);
            storyboardOpacity.Begin(element);
        }

        public static void AnimateFallingObject(Target target, int targetSpeed = GameConstants.Target.SpeedFalling)
        {
            var animationFalling = new DoubleAnimation
            {
                From = GameConstants.Target.MinPositionY,
                To = GameConstants.Canvas.HeightMax + 200,
                Duration = new Duration(TimeSpan.FromSeconds(targetSpeed)),
                AutoReverse = false
            };

            Storyboard.SetTargetName(animationFalling, target.Label.Name);
            Storyboard.SetTargetProperty(animationFalling, new PropertyPath(Canvas.TopProperty));

            // Create a storyboard to contain the animation.
            Storyboard storyboardFalling = new Storyboard();
            storyboardFalling.Children.Add(animationFalling);
            storyboardFalling.Begin(target.Label);
        }
    }
}
