using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace POEChatBotApplication
{
    public partial class SplashScreen : Window
    {
        private DispatcherTimer _progressTimer;
        private DispatcherTimer _statusTimer;
        private double _progressAngle = 0;
        private int _loadingPercent = 0;
        private string[] _statusMessages = {
            "⚡ INITIALIZING NEURAL NETWORK...",
            "🔐 ESTABLISHING SECURE CONNECTION...",
            "🧠 LOADING CYBERSECURITY DATABASE...",
            "🛡️ ACTIVATING FIREWALL PROTOCOLS...",
            "🔍 SCANNING FOR THREATS...",
            "✅ SYSTEM READY!",
            "🚀 LAUNCHING CYBER-BOT..."
        };
        private int _statusIndex = 0;

        public SplashScreen()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Start the main progress timer
            _progressTimer = new DispatcherTimer();
            _progressTimer.Interval = TimeSpan.FromMilliseconds(16); // ~60fps
            _progressTimer.Tick += ProgressTimer_Tick;
            _progressTimer.Start();

            // Start the status message changer
            _statusTimer = new DispatcherTimer();
            _statusTimer.Interval = TimeSpan.FromMilliseconds(800);
            _statusTimer.Tick += StatusTimer_Tick;
            _statusTimer.Start();

            // Start subtle animations
            StartPulsingAnimations();
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            _progressAngle += 2.5;

            // Calculate percentage (0-100)
            _loadingPercent = (int)((_progressAngle / 360) * 100);
            if (_loadingPercent > 100) _loadingPercent = 100;

            // Update loading percentage
            LoadingPercent.Text = $"{_loadingPercent}%";

            // Update progress arc
            if (_progressAngle >= 360)
            {
                _progressTimer.Stop();
                _statusTimer.Stop();
                LoadingStatus.Text = "🚀 LAUNCHING CYBER-BOT...";
                LoadingPercent.Text = "100%";

                // Slight delay before opening main window
                var closeTimer = new DispatcherTimer();
                closeTimer.Interval = TimeSpan.FromMilliseconds(500);
                closeTimer.Tick += (s, args) =>
                {
                    closeTimer.Stop();
                    OpenMainWindow();
                };
                closeTimer.Start();
                return;
            }

            // Update the arc path
            double radians = (_progressAngle - 90) * Math.PI / 180.0;
            double center = 100;
            double radius = 94;
            double x = center + radius * Math.Cos(radians);
            double y = center + radius * Math.Sin(radians);

            ProgressArcSegment.Point = new Point(x, y);
            ProgressArcSegment.IsLargeArc = _progressAngle >= 180;

            // Update ring rotations (make sure these exist in XAML)
            var ring1 = FindName("RingRotate1") as RotateTransform;
            var ring2 = FindName("RingRotate2") as RotateTransform;
            var ring3 = FindName("RingRotate3") as RotateTransform;

            if (ring1 != null) ring1.Angle = _progressAngle * 0.5;
            if (ring2 != null) ring2.Angle = -_progressAngle * 0.3;
            if (ring3 != null) ring3.Angle = _progressAngle * 0.7;

            // Pulse the lock icon slightly
            if (LockScale != null)
            {
                double pulseScale = 1 + Math.Sin(_progressAngle * 0.05) * 0.05;
                LockScale.ScaleX = pulseScale;
                LockScale.ScaleY = pulseScale;
            }

            // Animate dots
            if (DotsScale != null)
            {
                DotsScale.ScaleX = 1 + Math.Sin(_progressAngle * 0.08) * 0.1;
                DotsScale.ScaleY = 1 + Math.Sin(_progressAngle * 0.08) * 0.1;
            }

            // Update progress arc glow
            double glowOpacity = 0.5 + Math.Sin(_progressAngle * 0.05) * 0.3;
            ProgressArc.Opacity = glowOpacity;
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            if (_loadingPercent >= 100) return;

            _statusIndex = (_statusIndex + 1) % _statusMessages.Length;
            LoadingStatus.Text = _statusMessages[_statusIndex];

            // Change color based on status
            if (_statusIndex == 0)
                LoadingStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 0)); // Yellow
            else if (_statusIndex == 4)
                LoadingStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 165, 0)); // Orange
            else if (_statusIndex >= 5)
                LoadingStatus.Foreground = new SolidColorBrush(Color.FromRgb(50, 255, 50)); // LimeGreen
            else
                LoadingStatus.Foreground = new SolidColorBrush(Color.FromRgb(144, 238, 144)); // LightGreen

            // Add a fade effect
            var fadeAnimation = new DoubleAnimation
            {
                From = 0.3,
                To = 1.0,
                Duration = TimeSpan.FromMilliseconds(300),
                AutoReverse = true
            };
            LoadingStatus.BeginAnimation(TextBlock.OpacityProperty, fadeAnimation);
        }

        private void StartPulsingAnimations()
        {
            // Pulse the lock icon continuously
            if (LockScale != null)
            {
                var pulseAnimation = new DoubleAnimation
                {
                    From = 0.9,
                    To = 1.1,
                    Duration = TimeSpan.FromSeconds(1.5),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };
                LockScale.BeginAnimation(ScaleTransform.ScaleXProperty, pulseAnimation);
                LockScale.BeginAnimation(ScaleTransform.ScaleYProperty, pulseAnimation);
            }

            // Pulse the dots
            if (DotsScale != null)
            {
                var dotsAnimation = new DoubleAnimation
                {
                    From = 0.8,
                    To = 1.2,
                    Duration = TimeSpan.FromSeconds(2),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };
                DotsScale.BeginAnimation(ScaleTransform.ScaleXProperty, dotsAnimation);
                DotsScale.BeginAnimation(ScaleTransform.ScaleYProperty, dotsAnimation);
            }

            // Pulse the progress arc glow
            var glowAnimation = new DoubleAnimation
            {
                From = 0.6,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(1.2),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            ProgressArc.BeginAnimation(UIElement.OpacityProperty, glowAnimation);
        }

        private void OpenMainWindow()
        {
            try
            {
                // Create and show the main window
                MainWindow main = new MainWindow();
                main.Show();

                // Close the splash screen
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening main window: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}