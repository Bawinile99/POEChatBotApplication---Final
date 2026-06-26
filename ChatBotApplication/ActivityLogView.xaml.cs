using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace POEChatBotApplication.Views
{
    public partial class ActivityLogView : UserControl
    {
        public ObservableCollection<ActivityLogEntry> ActivityLog { get; set; }

        public ActivityLogView()
        {
            InitializeComponent();
            ActivityLog = SharedActivityLog.Instance.LogEntries;
            DataContext = this;
            SharedActivityLog.Instance.AddEntry("Opened Activity Log view", "Log");
        }

        private void ClearLog_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all activity logs?", "Confirm",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SharedActivityLog.Instance.Clear();
                SharedActivityLog.Instance.AddEntry("Activity log cleared by user", "Log");
                // Refresh the view
                ActivityLog = SharedActivityLog.Instance.LogEntries;
                DataContext = null;
                DataContext = this;
            }
        }

        private void FilterLog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var filter = selectedItem.Content?.ToString();
                ActivityLog = SharedActivityLog.Instance.FilterByCategory(filter);
                DataContext = null;
                DataContext = this;
            }
        }

        private void BackgroundVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Loop the background video
            if (BackgroundVideo != null)
            {
                BackgroundVideo.Position = TimeSpan.Zero;
                BackgroundVideo.Play();
            }
        }
    }

    public class ActivityLogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }

    public class SharedActivityLog
    {
        private static SharedActivityLog _instance;
        public static SharedActivityLog Instance => _instance ??= new SharedActivityLog();
        public ObservableCollection<ActivityLogEntry> LogEntries { get; set; }

        private SharedActivityLog()
        {
            LogEntries = new ObservableCollection<ActivityLogEntry>();
        }

        public void AddEntry(string description, string category = "General")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                LogEntries.Add(new ActivityLogEntry
                {
                    Timestamp = DateTime.Now,
                    Description = description,
                    Category = category
                });
            });
        }

        public void Clear()
        {
            Application.Current.Dispatcher.Invoke(() => LogEntries.Clear());
        }

        public ObservableCollection<ActivityLogEntry> FilterByCategory(string category)
        {
            if (string.IsNullOrEmpty(category) || category == "All")
                return LogEntries;

            var filtered = LogEntries.Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            return new ObservableCollection<ActivityLogEntry>(filtered);
        }
    }
}