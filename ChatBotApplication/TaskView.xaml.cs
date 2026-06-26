using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace POEChatBotApplication.Views
{
    public partial class TaskView : UserControl
    {
        public ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();
        private string connectionString = "Server=localhost;Database=cyberbot_db;Uid=root;Pwd=;";
        private bool isEditMode = false;
        private Task editingTask = null;

        public TaskView()
        {
            InitializeComponent();
            TaskListView.ItemsSource = Tasks;
            LoadTasksFromDatabase();
        }

        private void LoadTasksFromDatabase()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM tasks ORDER BY IsCompleted ASC, ReminderDate ASC";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        Tasks.Clear();
                        while (reader.Read())
                        {
                            Tasks.Add(new Task
                            {
                                Id = reader.GetInt32("Id"),
                                Title = reader.GetString("Title"),
                                Description = reader.GetString("Description"),
                                ReminderDate = reader.GetDateTime("ReminderDate"),
                                IsCompleted = reader.GetBoolean("IsCompleted")
                            });
                        }
                    }
                }
                SharedActivityLog.Instance.AddEntry($"Loaded {Tasks.Count} tasks from database", "Task");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskTitleBox.Text))
            {
                MessageBox.Show("Please enter a title for the task.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (isEditMode && editingTask != null)
            {
                UpdateTaskInDatabase();
                return;
            }

            Task newTask = new Task
            {
                Title = TaskTitleBox.Text.Trim(),
                Description = TaskDescriptionBox.Text.Trim(),
                ReminderDate = ReminderDatePicker.SelectedDate ?? DateTime.Today.AddDays(7),
                IsCompleted = false
            };

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO tasks (Title, Description, ReminderDate, IsCompleted) 
                                    VALUES (@Title, @Description, @ReminderDate, @IsCompleted)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Title", newTask.Title);
                    cmd.Parameters.AddWithValue("@Description", newTask.Description);
                    cmd.Parameters.AddWithValue("@ReminderDate", newTask.ReminderDate);
                    cmd.Parameters.AddWithValue("@IsCompleted", newTask.IsCompleted);
                    cmd.ExecuteNonQuery();
                }
                Tasks.Add(newTask);
                SharedActivityLog.Instance.AddEntry($"Task added: '{newTask.Title}'", "Task");
                ClearInputFields();
                LoadTasksFromDatabase();
                MessageBox.Show("Task added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding task: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTaskInDatabase()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE tasks SET Title = @Title, Description = @Description, ReminderDate = @ReminderDate 
                                    WHERE Id = @Id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Title", TaskTitleBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", TaskDescriptionBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@ReminderDate", ReminderDatePicker.SelectedDate ?? DateTime.Today.AddDays(7));
                    cmd.Parameters.AddWithValue("@Id", editingTask.Id);
                    cmd.ExecuteNonQuery();
                }
                SharedActivityLog.Instance.AddEntry($"Task updated: '{TaskTitleBox.Text.Trim()}'", "Task");
                ClearInputFields();
                LoadTasksFromDatabase();
                isEditMode = false;
                editingTask = null;
                MessageBox.Show("Task updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating task: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Task selectedTask)
            {
                isEditMode = true;
                editingTask = selectedTask;
                TaskTitleBox.Text = selectedTask.Title;
                TaskDescriptionBox.Text = selectedTask.Description;
                ReminderDatePicker.SelectedDate = selectedTask.ReminderDate;
                MessageBox.Show($"Editing task: '{selectedTask.Title}'. Update the fields and click 'Add Task' to save.",
                              "Edit Mode", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (TaskListView.SelectedItem is Task selectedTaskFromList)
            {
                isEditMode = true;
                editingTask = selectedTaskFromList;
                TaskTitleBox.Text = selectedTaskFromList.Title;
                TaskDescriptionBox.Text = selectedTaskFromList.Description;
                ReminderDatePicker.SelectedDate = selectedTaskFromList.ReminderDate;
                MessageBox.Show($"Editing task: '{selectedTaskFromList.Title}'. Update the fields and click 'Add Task' to save.",
                              "Edit Mode", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select a task to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            Task selectedTask = null;

            if (sender is Button button && button.Tag is Task taskFromTag)
            {
                selectedTask = taskFromTag;
            }
            else if (TaskListView.SelectedItem is Task taskFromList)
            {
                selectedTask = taskFromList;
            }

            if (selectedTask != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete task '{selectedTask.Title}'?",
                                           "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            string query = "DELETE FROM tasks WHERE Id = @Id";
                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@Id", selectedTask.Id);
                            cmd.ExecuteNonQuery();
                        }
                        Tasks.Remove(selectedTask);
                        SharedActivityLog.Instance.AddEntry($"Task deleted: '{selectedTask.Title}'", "Task");
                        LoadTasksFromDatabase();
                        MessageBox.Show("Task deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting task: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a task to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClearInputFields()
        {
            TaskTitleBox.Text = string.Empty;
            TaskDescriptionBox.Text = string.Empty;
            ReminderDatePicker.SelectedDate = null;
            isEditMode = false;
            editingTask = null;
        }
    }
}