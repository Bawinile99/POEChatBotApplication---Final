using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace POEChatBotApplication.Views
{
    public partial class QuizView : UserControl
    {
        private List<Question> Questions = new();
        private int currentQuestionIndex = 0;
        private int score = 0;
        private bool quizCompleted = false;
        private int questionsAnswered = 0;

        public QuizView()
        {
            InitializeComponent();
            LoadQuestions();
            DisplayCurrentQuestion();
            SharedActivityLog.Instance.AddEntry("Quiz started", "Quiz");
        }

        private void LoadQuestions()
        {
            Questions = new List<Question>
            {
                new Question("What is phishing?",
                    new[] { "Fishing in a lake", "A cyberattack to steal personal info", "A type of computer virus", "A coding language" }, 1,
                    "Phishing is a cyberattack where criminals impersonate legitimate organizations to steal sensitive information like passwords and credit card numbers."),

                new Question("What should you do with suspicious emails?",
                    new[] { "Open all attachments", "Click all links", "Report as spam/phishing and delete", "Forward to friends" }, 2,
                    "Suspicious emails should be reported as spam/phishing. Never click links or open attachments from unknown senders."),

                new Question("What makes a strong password?",
                    new[] { "Using 'password123'", "Your pet's name with year", "Random mix of uppercase, lowercase, numbers, and symbols", "Your birthday" }, 2,
                    "Strong passwords should be at least 12 characters long and include a mix of uppercase, lowercase, numbers, and special characters."),

                new Question("What does HTTPS mean for a website?",
                    new[] { "Hyper Text Transfer Protocol", "Hacker Test Transfer Protocol", "Secure connection encrypting your data", "High Tech Transfer System" }, 2,
                    "HTTPS ensures encrypted communication between your browser and the website, protecting your data from interception."),

                new Question("What is malware?",
                    new[] { "A type of fish", "Good computer software", "Malicious software designed to harm devices", "A computer brand" }, 2,
                    "Malware includes viruses, trojans, ransomware, and spyware that can steal data or damage your system."),

                new Question("Why use antivirus software?",
                    new[] { "Makes computer faster", "Protects against viruses and malware", "Changes screen color", "Creates passwords" }, 1,
                    "Antivirus software detects, prevents, and removes malware, protecting your system and data."),

                new Question("What is 2FA (Two-Factor Authentication)?",
                    new[] { "Two Friends Authentication", "Two-Factor Authentication adds an extra security layer", "Two File Access", "Too Fast Authentication" }, 1,
                    "2FA requires two forms of verification (e.g., password + code from phone app) to access an account, adding an extra security layer."),

                new Question("What information is safe to share online?",
                    new[] { "Your passwords", "Personal identification details", "Only what's necessary and non-sensitive", "Your home address" }, 2,
                    "Never share sensitive information like passwords, addresses, or ID numbers. Share only what's necessary for the situation."),

                new Question("What is social engineering?",
                    new[] { "A type of engineering", "Manipulating people to reveal sensitive information", "Building social networks", "A programming technique" }, 1,
                    "Social engineering uses psychological manipulation to trick people into giving away confidential information or access."),

                new Question("True or False: Public Wi-Fi is always safe for banking.",
                    new[] { "True", "False" }, 1,
                    "Public Wi-Fi is not secure. Always use a VPN when accessing sensitive accounts on public networks."),

                new Question("What is ransomware?",
                    new[] { "A type of advertisement", "Malware that encrypts files and demands payment", "Anti-virus software", "Browser extension" }, 1,
                    "Ransomware is malware that encrypts your files and demands payment (ransom) to restore access."),

                new Question("How often should you update your software?",
                    new[] { "Never", "Only when prompted", "Regularly as updates become available", "Once a year" }, 2,
                    "Regular updates are crucial as they fix security vulnerabilities and protect against new threats."),

                new Question("True or False: Using the same password for multiple accounts is safe.",
                    new[] { "True", "False" }, 1,
                    "Never reuse passwords across accounts. If one account is compromised, all accounts using the same password are at risk."),

                new Question("What is a VPN?",
                    new[] { "Virtual Private Network - Secures your connection", "Very Personal Network", "Virtual Public Network", "Video Processing Network" }, 0,
                    "A VPN (Virtual Private Network) encrypts your internet connection, protecting your privacy and data, especially on public Wi-Fi."),

                new Question("True or False: You should use the same password for all your accounts to remember them easily.",
                    new[] { "True", "False" }, 1,
                    "Never reuse passwords across accounts. Use a password manager to store unique, strong passwords for each account.")
            };
        }

        private void DisplayCurrentQuestion()
        {
            if (currentQuestionIndex >= Questions.Count)
            {
                ShowFinalFeedback();
                quizCompleted = true;
                return;
            }

            quizCompleted = false;
            Question current = Questions[currentQuestionIndex];
            QuestionText.Text = current.Text;
            OptionsPanel.Items.Clear();
            QuestionProgress.Text = $"Question {currentQuestionIndex + 1}/{Questions.Count}";

            FeedbackText.Text = ">_ SELECT YOUR ANSWER...";
            FeedbackText.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            ExplanationBorder.Visibility = Visibility.Collapsed;

            for (int i = 0; i < current.Options.Length; i++)
            {
                var radio = new RadioButton
                {
                    Content = current.Options[i],
                    Tag = i,
                    GroupName = "QuizOptions",
                    Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White),
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 5, 0, 5),
                    Style = (Style)Resources["CyberHackerRadio"]
                };
                OptionsPanel.Items.Add(radio);
            }

            SubmitAnswerButton.Content = currentQuestionIndex == Questions.Count - 1 ? "FINISH QUIZ" : "SUBMIT ANSWER";
        }

        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (quizCompleted)
            {
                ResetQuiz();
                return;
            }

            var selectedOption = OptionsPanel.Items.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);
            if (selectedOption == null)
            {
                MessageBox.Show("Please select an answer.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int selectedIndex = (int)selectedOption.Tag;
            Question current = Questions[currentQuestionIndex];
            bool isCorrect = selectedIndex == current.CorrectOption;

            // Show feedback immediately
            string correctOptionText = current.Options[current.CorrectOption];
            ExplanationBorder.Visibility = Visibility.Visible;

            if (isCorrect)
            {
                score++;
                FeedbackText.Text = "✅ CORRECT! Well done!";
                FeedbackText.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LimeGreen);
                ExplanationText.Text = $"📝 {current.Explanation}";
                SharedActivityLog.Instance.AddEntry($"Quiz - Correct answer: {current.Text}", "Quiz");
            }
            else
            {
                FeedbackText.Text = $"❌ INCORRECT. The correct answer was: {correctOptionText}";
                FeedbackText.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                ExplanationText.Text = $"📝 {current.Explanation}";
                SharedActivityLog.Instance.AddEntry($"Quiz - Incorrect answer: {current.Text}", "Quiz");
            }

            questionsAnswered++;
            ScoreText.Text = $"{score}/{Questions.Count}";
            ScoreProgressBar.Value = (double)score / Questions.Count * 100;
            currentQuestionIndex++;
            DisplayCurrentQuestion();
        }

        private void ShowFinalFeedback()
        {
            QuestionText.Text = "🛡️ CYBERSECURITY QUIZ COMPLETE!";
            OptionsPanel.Items.Clear();
            SubmitAnswerButton.Content = "🔄 TAKE QUIZ AGAIN";
            QuestionProgress.Text = "COMPLETED!";

            double percentage = (double)score / Questions.Count * 100;
            ScoreText.Text = $"{score}/{Questions.Count} ({percentage:F0}%)";
            ScoreProgressBar.Value = percentage;

            string feedback;
            if (percentage >= 90)
                feedback = "🏆 ELITE CYBERSECURITY PRO! You're a digital security expert!";
            else if (percentage >= 75)
                feedback = "🔐 SECURITY CHAMPION! Great job! You're a cybersecurity pro!";
            else if (percentage >= 60)
                feedback = "📚 GOOD AWARENESS! Keep learning to stay safe online!";
            else
                feedback = "📖 KEEP LEARNING! Review the answers and try again!";

            FeedbackText.Text = feedback;
            FeedbackText.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Cyan);
            ExplanationBorder.Visibility = Visibility.Visible;
            ExplanationText.Text = "🎯 Review the questions you got wrong to improve your cybersecurity knowledge!";

            SharedActivityLog.Instance.AddEntry($"Quiz completed with score {score}/{Questions.Count} - {questionsAnswered} questions answered", "Quiz");
            quizCompleted = true;
        }

        private void ResetQuiz()
        {
            currentQuestionIndex = 0;
            score = 0;
            questionsAnswered = 0;
            quizCompleted = false;
            ScoreText.Text = "0/15";
            ScoreProgressBar.Value = 0;
            LoadQuestions();
            DisplayCurrentQuestion();
            SharedActivityLog.Instance.AddEntry("Quiz restarted", "Quiz");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize colors and set up the UI when the control loads
            QuestionText.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            ScoreText.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            FeedbackText.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            QuestionProgress.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);

            // Set initial progress
            ScoreProgressBar.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            ScoreProgressBar.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
            ScoreProgressBar.Maximum = Questions.Count;

            // Ensure the submit button text is correct
            if (Questions.Count > 0 && currentQuestionIndex < Questions.Count)
            {
                SubmitAnswerButton.Content = currentQuestionIndex == Questions.Count - 1 ? "FINISH QUIZ" : "SUBMIT ANSWER";
            }
        }
    }

    public class Question
    {
        public string Text { get; set; }
        public string[] Options { get; set; }
        public int CorrectOption { get; set; }
        public string Explanation { get; set; }

        public Question(string text, string[] options, int correctOption, string explanation = "")
        {
            Text = text;
            Options = options;
            CorrectOption = correctOption;
            Explanation = explanation;
        }
    }
}