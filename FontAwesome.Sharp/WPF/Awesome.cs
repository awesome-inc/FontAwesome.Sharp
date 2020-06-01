using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FontAwesome.Sharp
{
    public static class Awesome
    {
        #region Animation

        public static readonly DependencyProperty SpinProperty = DependencyProperty.RegisterAttached("Spin",
            typeof(bool), typeof(Awesome), new PropertyMetadata(false, SpinChanged));

        public static readonly DependencyProperty SpinDurationProperty =
            DependencyProperty.RegisterAttached("SpinDuration",
                typeof(double), typeof(Awesome),
                new PropertyMetadata(1.0d, SpinDurationChanged, SpinDurationCoerceValue));

        public static readonly DependencyProperty RotationProperty = DependencyProperty.RegisterAttached("Rotation",
            typeof(double), typeof(Awesome), new PropertyMetadata(0.0d, RotationChanged, RotationCoerceValue));

        public static readonly DependencyProperty FlipProperty = DependencyProperty.RegisterAttached("Flip",
            typeof(FlipOrientation), typeof(Awesome), new PropertyMetadata(FlipOrientation.Normal, FlipChanged));


        // ReSharper disable once UnusedMember.Global
        public static bool GetSpin(DependencyObject target)
        {
            return (bool)target.GetValue(SpinProperty);
        }

        public static void SetSpin(DependencyObject target, bool value)
        {
            target.SetValue(SpinProperty, value);
        }

        public static double GetSpinDuration(DependencyObject target)
        {
            return (double)target.GetValue(SpinDurationProperty);
        }

        public static void SetSpinDuration(DependencyObject target, double value)
        {
            target.SetValue(SpinDurationProperty, value);
        }

        public static double GetRotation(DependencyObject target)
        {
            return (double)target.GetValue(RotationProperty);
        }

        public static void SetRotation(DependencyObject target, double value)
        {
            target.SetValue(RotationProperty, value);
        }

        // ReSharper disable once UnusedMember.Global
        public static FlipOrientation GetFlip(DependencyObject target)
        {
            return (FlipOrientation)target.GetValue(FlipProperty);
        }

        public static void SetFlip(DependencyObject target, FlipOrientation value)
        {
            target.SetValue(FlipProperty, value);
        }

        private static void SpinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement control)) return;

            if (!(e.NewValue is bool) || e.NewValue.Equals(e.OldValue)) return;
            var spin = (bool)e.NewValue;

            if (spin)
            {
                var spinDuration = GetSpinDuration(control);
                BeginSpin(control, spinDuration);
            }
            else
            {
                StopSpin(control);
                var rotation = GetRotation(control);
                SetRotation(control, rotation);
            }
        }

        private static void SpinDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement control)) return;
            if (!(e.NewValue is double) || e.NewValue.Equals(e.OldValue)) return;
            var spinDuration = (double)e.NewValue;
            StopSpin(control);
            BeginSpin(control, spinDuration);
        }

        private static object SpinDurationCoerceValue(DependencyObject d, object value)
        {
            var val = (double)value;
            return val < 0 ? 0d : value;
        }

        private static void RotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement control)) return;
            if (!(e.NewValue is double) || e.NewValue.Equals(e.OldValue)) return;
            var rotation = (double)e.NewValue;
            SetRotation(control, rotation);
        }

        private static object RotationCoerceValue(DependencyObject d, object value)
        {
            return Math.Max(0.0, Math.Min(360.0, (double)value));
        }

        private static void FlipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement control)) return;
            if (!(e.NewValue is FlipOrientation) || e.NewValue.Equals(e.OldValue)) return;
            var flipOrientation = (FlipOrientation)e.NewValue;
            SetFlipOrientation(control, flipOrientation);
        }

        private static readonly string SpinnerStoryBoardName = $"{typeof(Awesome).Namespace}.SpinnerStoryBoard";

        private static void BeginSpin(FrameworkElement control, double duration)
        {
            var transformGroup = control.RenderTransform as TransformGroup ?? new TransformGroup();
            var rotateTransform = transformGroup.Children.OfType<RotateTransform>().FirstOrDefault();

            if (rotateTransform != null)
            {
                rotateTransform.Angle = 0;
            }
            else
            {
                transformGroup.Children.Add(new RotateTransform(0.0));
                control.RenderTransform = transformGroup;
                control.RenderTransformOrigin = new Point(0.5, 0.5);
            }


            var storyboard = GetStoryboard(control);
            if (storyboard != null) return;

            storyboard = new Storyboard();

            var initialRotation = GetRotation(control);
            var animation = new DoubleAnimation
            {
                From = initialRotation,
                To = initialRotation + 360.0,
                AutoReverse = false,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(TimeSpan.FromSeconds(duration))
            };
            storyboard.Children.Add(animation);

            Storyboard.SetTarget(animation, control);
            Storyboard.SetTargetProperty(animation,
                new PropertyPath("(0).(1)[0].(2)", UIElement.RenderTransformProperty,
                    TransformGroup.ChildrenProperty, RotateTransform.AngleProperty));

            storyboard.Begin();
            control.Resources.Add(SpinnerStoryBoardName, storyboard);
        }

        private static void StopSpin(FrameworkElement control)
        {
            var storyboard = GetStoryboard(control);
            if (storyboard == null) return;
            storyboard.Stop();
            control.Resources.Remove(SpinnerStoryBoardName);
        }

        private static void SetRotation(UIElement control, double rotation)
        {
            var transformGroup = control.RenderTransform as TransformGroup ?? new TransformGroup();
            var rotateTransform = transformGroup.Children.OfType<RotateTransform>().FirstOrDefault();
            if (rotateTransform != null)
            {
                rotateTransform.Angle = rotation;
            }
            else
            {
                transformGroup.Children.Add(new RotateTransform(rotation));
                control.RenderTransform = transformGroup;
                control.RenderTransformOrigin = new Point(0.5, 0.5);
            }
        }

        private static void SetFlipOrientation(UIElement control, FlipOrientation flipOrientation)
        {
            var transformGroup = control.RenderTransform as TransformGroup ?? new TransformGroup();
            var scaleX = flipOrientation == FlipOrientation.Horizontal ? -1 : 1;
            var scaleY = flipOrientation == FlipOrientation.Vertical ? -1 : 1;
            var scaleTransform = transformGroup.Children.OfType<ScaleTransform>().FirstOrDefault();
            if (scaleTransform != null)
            {
                scaleTransform.ScaleX = scaleX;
                scaleTransform.ScaleY = scaleY;
            }
            else
            {
                transformGroup.Children.Add(new ScaleTransform(scaleX, scaleY));
                control.RenderTransform = transformGroup;
                control.RenderTransformOrigin = new Point(0.5, 0.5);
            }
        }

        private static Storyboard GetStoryboard(FrameworkElement control)
        {
            return control.Resources[SpinnerStoryBoardName] as Storyboard;
        }

        #endregion Animation

        #region Inline text

        public static readonly DependencyProperty InlineProperty = DependencyProperty.RegisterAttached(
            "Inline", typeof(string), typeof(Awesome),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure,
                InlinePropertyChanged));

        public const string DefaultPattern = @":(\w+):";

        public static readonly DependencyProperty PatternProperty = DependencyProperty.RegisterAttached(
            "Pattern", typeof(string), typeof(Awesome), new PropertyMetadata(DefaultPattern));

        public static void SetInline(DependencyObject textBlock, string value)
        {
            textBlock.SetValue(InlineProperty, value);
        }

        // ReSharper disable once UnusedMember.Global
        public static string GetInline(DependencyObject textBlock)
        {
            return (string)textBlock.GetValue(InlineProperty);
        }

        public static void SetPattern(DependencyObject textBlock, string value)
        {
            textBlock.SetValue(PatternProperty, value);
        }

        public static string GetPattern(DependencyObject textBlock)
        {
            return (string)textBlock.GetValue(PatternProperty);
        }

        private static void InlinePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBlock textBlock))
                return;

            var text = (string)e.NewValue ?? string.Empty;
            var pattern = GetPattern(textBlock) ?? DefaultPattern;
            var inLines = FormatText(text, pattern).ToList();

            if (inLines.Any())
            {
                textBlock.Inlines.Clear();
                inLines.ForEach(textBlock.Inlines.Add);
            }
            else
            {
                textBlock.Text = text;
            }
        }

        public static IEnumerable<Inline> FormatText(string text, string pattern = DefaultPattern)
        {
            var tokens = Regex.Split(text, pattern);
            if (tokens.Length == 1) return Enumerable.Empty<Inline>();

            var inlines = new List<Inline>();
            for (var i = 0; i < tokens.Length; i += 2)
            {
                var t = tokens[i];
                if (!string.IsNullOrWhiteSpace(t))
                    inlines.Add(new Run(t));
                if (i + 1 >= tokens.Length) break;

                t = tokens[i + 1];
                inlines.Add(RunFor(t));
            }

            return inlines;
        }

        private static Run RunFor(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("token must not be null, empty or whitespace");
            return Enum.TryParse<IconChar>(token, true, out var icon)
                ? new Run(icon.ToChar()) { FontFamily = IconHelper.FontFor(icon) }
                : new Run(token);
        }

        #endregion Inline text
    }
}
