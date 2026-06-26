using System.Windows;
using System.Windows.Controls;
using POEChatBotApplication.Views; // Ensure this includes your UserControls

namespace POEChatBotApplication
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                Window viewWindow = null;

                switch (tag)
                {
                    case "TaskView":
                        SharedActivityLog.Instance.AddEntry("Visited the Task Assistant page", "Task");
                        viewWindow = CreateViewWindow(new TaskView(), "Task Assistant");
                        break;
                    case "QuizView":
                        SharedActivityLog.Instance.AddEntry("Opened the Quiz Mini-Game", "Quiz");
                        viewWindow = CreateViewWindow(new QuizView(), "Quiz Mini-Game");
                        break;
                    case "ChatView":
                        SharedActivityLog.Instance.AddEntry("Entered the Chat Assistant", "Chat");
                        viewWindow = CreateViewWindow(new ChatView(), "Chat Assistant");
                        break;
                    case "ActivityLogView":
                        SharedActivityLog.Instance.AddEntry("Viewed the Activity Log summary", "Log");
                        viewWindow = CreateViewWindow(new ActivityLogView(), "Activity Log");
                        break;
                }

                viewWindow?.Show();
            }
        }

        private Window CreateViewWindow(UserControl viewControl, string title)
        {
            return new Window
            {
                Title = title,
                Content = viewControl,
                Width = 1000,
                Height = 700,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.CanResize,
                WindowStyle = WindowStyle.ToolWindow
            };
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void BackgroundVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundVideo.Position = TimeSpan.Zero;
            BackgroundVideo.Play();
        }

    }
}