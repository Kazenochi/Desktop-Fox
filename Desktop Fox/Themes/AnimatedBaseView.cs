using DesktopFox.MVVM.Views;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DesktopFox
{
    public class AnimatedBaseView : UserControl
    {
        public Animation ControlPopInAnimation { get; set; } = Animation.PopInAnimation;
        public Animation ControlPopOutAnimation { get; set; } = Animation.PopOutAnimation;
        public Animation ControlFadeInAnimation { get; set; } = Animation.FadeInAnimation;
        public Animation ControlFadeOutAnimation { get; set; } = Animation.FadeOutAnimation;

        public double AnimationTime { get; set; } = 0.5;
        public double Amplitude { get; set; } = 0.3;

        public AnimatedBaseView()
        {
        }

        /// <summary>
        /// Easing Effekt für PopOut des Elements in der angegebenen Zeit <see cref="AnimationTime"/>
        /// </summary>
        public void AnimateIn()
        {
            if (this.ControlPopInAnimation == Animation.None)
                return;

            ScaleTransform scaleTransform = new ScaleTransform();
            this.RenderTransformOrigin = new Point (1,1);
            this.RenderTransform = scaleTransform;
                    
            var sb = new Storyboard();
            var popinAnimationX = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(this.AnimationTime));

            BackEase quadraticEase = new BackEase();
            quadraticEase.EasingMode = EasingMode.EaseOut;
            quadraticEase.Amplitude = Amplitude;
            popinAnimationX.EasingFunction = quadraticEase;

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, popinAnimationX);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, popinAnimationX);
        }

        /// <summary>
        /// Easing Effekt für PopIn des des Elements in der angegebenen Zeit <see cref="AnimationTime"/>
        /// </summary>
        public void AnimateOut()
        {
            if (this.ControlPopOutAnimation == Animation.None)
                return;

            ScaleTransform scaleTransform = new ScaleTransform();
            this.RenderTransformOrigin = new Point(1, 1);
            this.RenderTransform = scaleTransform;

            var sb = new Storyboard();
            var popinAnimationX = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(this.AnimationTime));

            BackEase quadraticEase = new BackEase();
            quadraticEase.EasingMode = EasingMode.EaseIn;
            quadraticEase.Amplitude = Amplitude;
            popinAnimationX.EasingFunction = quadraticEase;
                    
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, popinAnimationX);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, popinAnimationX);
        }

        /// <summary>
        /// Erhöht die Deckkraft des Elements in der angegebenen Zeit <see cref="AnimationTime"/>
        /// </summary>
        public void FadeIn()
        {
            if (this.ControlFadeInAnimation == Animation.None)
                return;
         
            this.Opacity = 0;
            this.Visibility = Visibility.Visible;

            var sb = new Storyboard();
            var fadeinAnimationX = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(this.AnimationTime));
            sb.Children.Add(fadeinAnimationX);
            Storyboard.SetTarget(sb, this);
            Storyboard.SetTargetProperty(sb, new PropertyPath(Control.OpacityProperty));
       
            sb.Begin();
            sb.Completed += delegate(object sender, EventArgs e) 
            {
                this.Visibility = Visibility.Collapsed;
            };

        }

        /// <summary>
        /// Verringert die Deckkraft des Elements in der angegebenen Zeit <see cref="AnimationTime"/>
        /// </summary>
        public void FadeOut()
        {
            if (this.ControlFadeOutAnimation == Animation.None)
                return;
        }
    }
}
