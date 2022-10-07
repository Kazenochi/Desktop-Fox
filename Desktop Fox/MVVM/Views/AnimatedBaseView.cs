﻿using DesktopFox.MVVM.Views;
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

        public double AnimationTime { get; set; } = 0.5;

        public AnimatedBaseView()
        {
        }

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
            popinAnimationX.EasingFunction = quadraticEase;

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, popinAnimationX);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, popinAnimationX);
        }

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
            popinAnimationX.EasingFunction = quadraticEase;
                    
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, popinAnimationX);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, popinAnimationX);
        }
    }
}
