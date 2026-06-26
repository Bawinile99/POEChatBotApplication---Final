using POEChatBotApplication.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace POEChatBotApplication.Views
{
    public partial class ChatView : UserControl
    {
        private string userName = string.Empty;
        private bool isNameCaptured = false;
        private bool hasShownTopicMenu = false;
        private bool inEnhancedChat = false;
        private string pendingTopic = string.Empty;

        private static readonly Dictionary<string, string> sentimentResponses = new()
        {
            { "happy", "It's wonderful to see you're happy! Let's keep that positive energy while we discuss security." },
            { "good", "Great to hear you're doing well! How can I assist you with cybersecurity today?" },
            { "great", "Fantastic! It's great to hear you're doing well. What security topics interest you?" },
            { "well", "I'm glad you're doing well! What security topics can we explore together?" },
            { "fine", "Good to hear you're doing fine. How can I help with your cybersecurity needs today?" },
            { "alright", "Alright! Let's get started with your cybersecurity questions." },
            { "okay", "Okay! Ready to discuss any security topics you have in mind." },
            { "excellent", "Excellent! You're in a great mood for learning about security!" },
            { "awesome", "Awesome! Let's channel this positive energy into security awareness!" },
            { "perfect", "Perfect! Let's make your digital security just as perfect!" },
            { "bad", "I'm sorry to hear that. Maybe I can help improve your day with some security tips." },
            { "sad", "I'm sorry you're feeling down. Security can be overwhelming, but I'm here to help." },
            { "worried", "It's completely understandable to feel that way. Let me share tips to help you stay safe." },
            { "angry", "I understand your frustration. Let's work through this security issue together." },
            { "confused", "Don't worry, cybersecurity can be confusing. I'll explain things clearly." },
            { "scared", "Your concerns are valid. I'll help you take steps to protect yourself." },
            { "excited", "I love your enthusiasm for security! Let's channel that energy into learning." },
            { "curious", "That's great! Curiosity is the first step to being informed and protected." },
            { "interested", "Wonderful that you're interested! Let's dive into security topics." },
            { "eager", "I love your eagerness! Let's satisfy your security curiosity." },
            { "bored", "Maybe it's a good time to sharpen your cybersecurity skills! Want a fun tip or quick quiz?" },
            { "tired", "Sounds like you've had a long day. Cybersecurity doesn't sleep though! Want something light, like browsing safety tips?" },
            { "anxious", "That's okay, many people feel overwhelmed online. Maybe I can help ease your concerns. Want a quick security checklist?" },
            { "stressed", "Stress is normal, but don't worry. I can help you feel more in control of your digital life." },
            { "tell me a joke", "Why did the hacker break up with the WiFi? ...Because it just wasn't secure anymore 😄" },
            { "who are you", "I'm CyberBot, your friendly neighborhood security sidekick 🤖. My mission? Helping you stay safe online!" },
            { "what can you do", "I can help you with passwords, phishing, VPNs, malware, hacked accounts, and more. Just ask about any of those!" },
            { "daily tip", "🗓️ Cyber Tip of the Day:\nAlways think before you click! Hover over suspicious links and check the URL before logging in." },
            { "give me motivation", "🔐 'Security is not a product, but a process.' – Bruce Schneier\nStay sharp, stay safe!" }
        };

        public ChatView()
        {
            InitializeComponent();
            Loaded += ChatView_Loaded;
        }

        private void ChatView_Loaded(object sender, RoutedEventArgs e)
        {
            AddMessageToChat("CyberBot", "Welcome to the Cybersecurity Assistant.", Brushes.Black);
            AddMessageToChat("CyberBot", "Enter your full name?", Brushes.Black);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = UserInputBox.Text.Trim();
            if (string.IsNullOrEmpty(userInput)) return;

            AddMessageToChat("You", userInput, Brushes.Black);
            string botResponse = GetBotResponse(userInput);
            AddMessageToChat("CyberBot", botResponse, Brushes.DarkRed);

            UserInputBox.Text = string.Empty;
        }

        private void AddMessageToChat(string senderName, string message, Brush background)
        {
            var border = new Border
            {
                Background = background,
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
                MaxWidth = 600,
                Child = new TextBlock
                {
                    Text = $"{senderName}: {message}",
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = Brushes.White,
                    FontSize = 14
                }
            };

            ChatList.Children.Add(border);
        }

        // ============ MAIN BOT RESPONSE METHOD ============
        private string GetBotResponse(string input)
        {
            input = input.ToLower().Trim();

            // Check for activity log requests first (Task 4 requirement)
            if (IsActivityLogRequest(input))
                return HandleActivityLogRequest();

            // Check for show more logs
            if (input.Contains("show more") || input.Contains("more logs") || input.Contains("full history"))
            {
                var moreLogs = HandleShowMoreLogs(input);
                if (moreLogs != null)
                    return moreLogs;
            }

            // Check for task/reminder commands (NLP simulation - Task 3)
            if (IsTaskRelatedCommand(input))
            {
                string taskText = ExtractTaskFromInput(input);
                SharedActivityLog.Instance.AddEntry($"NLP: Task creation requested - '{taskText}'", "NLP");
                return $"📋 Task added: '{taskText}.' Would you like to set a reminder for this task?";
            }

            if (IsReminderRelatedCommand(input))
            {
                string reminderText = ExtractReminderFromInput(input);
                SharedActivityLog.Instance.AddEntry($"NLP: Reminder set - '{reminderText}'", "NLP");
                return $"✅ {reminderText}";
            }

            // Existing logic continues...
            if (!isNameCaptured)
            {
                userName = input;
                isNameCaptured = true;
                SharedActivityLog.Instance.AddEntry($"User registered: {userName}", "Chat");
                return $"Nice to meet you, {userName}! Here's what I can help you with today:\n\n" +
                       "1. Password security and generation\n" +
                       "2. Phishing detection and prevention\n" +
                       "3. Safe browsing practices\n" +
                       "4. General cybersecurity tips\n" +
                       "5. Emergency security actions\n" +
                       "6. View activity log ('show activity log')\n" +
                       "7. Engage in a deeper cybersecurity chat";
            }

            // Enhanced chat mode - handles natural language questions
            if (inEnhancedChat)
            {
                if (input.Contains("exit"))
                {
                    inEnhancedChat = false;
                    SharedActivityLog.Instance.AddEntry("Exited enhanced chat", "Chat");
                    return "👍 Exiting enhanced chat. Let's go back to the main menu.";
                }

                // Check for topic expansion confirmation
                if (!string.IsNullOrEmpty(pendingTopic) && (input.Contains("yes") || input.Contains("sure") || input.Contains("please")))
                {
                    string expanded = TriggerTopicExpansion(pendingTopic);
                    string topic = pendingTopic;
                    pendingTopic = string.Empty;
                    SharedActivityLog.Instance.AddEntry($"Expanded topic: {topic}", "Chat");
                    return expanded + "\n\nI hope I have helped you with your request. You can now choose another topic to explore.";
                }

                // Check for sentiment responses
                foreach (var sentiment in sentimentResponses)
                {
                    if (input.Contains(sentiment.Key))
                    {
                        SharedActivityLog.Instance.AddEntry($"Sentiment detected: {sentiment.Key}", "Chat");
                        return sentiment.Value;
                    }
                }

                return HandleEnhancedConversation(input);
            }

            // Check if user wants to enter enhanced chat
            if (input == "7" || input.Contains("deep") || input.Contains("enhanced") || input.Contains("dive"))
            {
                return StartEnhancedChat();
            }

            // Main menu options - ONLY numbers 1-6
            if (input == "1")
                return HandlePasswordTopic();
            if (input == "2")
                return HandlePhishingTopic();
            if (input == "3")
                return HandleSafeBrowsingTopic();
            if (input == "4")
                return HandleGeneralSecurityTopic();
            if (input == "5")
                return HandleEmergencyTopic();
            if (input == "6")
                return HandleActivityLogRequest();
            if (input == "exit" || input == "goodbye" || input == "bye")
                return GetFarewellMessage();

            // If not in enhanced chat and not a menu option, try NLP
            string nlpResponse = ProcessNLPCommand(input);
            if (nlpResponse != null)
                return nlpResponse;

            // If still no match, ask user to be more specific or suggest enhanced chat
            return "❓ I didn't quite understand that. Would you like to:\n" +
                   "• Type a number (1-6) for specific topics\n" +
                   "• Type '7' for an enhanced chat where you can ask anything\n" +
                   "• Type 'show activity log' to see recent actions\n" +
                   "• Or ask me about passwords, phishing, VPNs, malware, or security in general";
        }

        // ============ ACTIVITY LOG METHODS ============
        private bool IsActivityLogRequest(string input)
        {
            string[] logTriggers = { "show activity log", "what have you done for me", "activity log", "show me the log", "what did you do", "show log", "log history" };
            foreach (var trigger in logTriggers)
            {
                if (input.Contains(trigger))
                    return true;
            }
            return false;
        }

        private string HandleActivityLogRequest()
        {
            var entries = SharedActivityLog.Instance.LogEntries;
            if (entries.Count == 0)
                return "📋 No activity logged yet. Start interacting with me to see actions here!";

            string response = "📋 Here's a summary of recent actions:\n\n";
            int count = Math.Min(10, entries.Count);
            int startIndex = Math.Max(0, entries.Count - count);

            for (int i = startIndex; i < entries.Count; i++)
            {
                var entry = entries[i];
                response += $"{i - startIndex + 1}. {entry.Description}";
                if (entry.Timestamp != null)
                    response += $" ({entry.Timestamp:MMM dd, yyyy})";
                response += $" [{entry.Category}]\n";
            }

            if (entries.Count > 10)
                response += "\n💡 Type 'show more' to see additional entries.";

            SharedActivityLog.Instance.AddEntry("Viewed activity log summary", "Log");
            return response;
        }

        private string HandleShowMoreLogs(string input)
        {
            if (!input.Contains("show more") && !input.Contains("more logs") && !input.Contains("full history"))
                return null;

            var entries = SharedActivityLog.Instance.LogEntries;
            if (entries.Count <= 10)
                return "That's all the entries we have!";

            string response = "📋 Full Activity History:\n\n";
            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                response += $"{i + 1}. {entry.Description}";
                if (entry.Timestamp != null)
                    response += $" ({entry.Timestamp:MMM dd, yyyy HH:mm})";
                response += $" [{entry.Category}]\n";
            }

            SharedActivityLog.Instance.AddEntry("Viewed full activity log history", "Log");
            return response;
        }

        // ============ NLP HELPER METHODS ============
        private string ExtractTaskFromInput(string input)
        {
            string[] removePhrases = { "add task", "new task", "create task", "add a task", "to ", "a ", "task " };
            string text = input;
            foreach (var phrase in removePhrases)
            {
                if (text.Contains(phrase))
                    text = text.Replace(phrase, "").Trim();
            }
            string[] articles = { "the ", "a ", "an " };
            foreach (var article in articles)
            {
                if (text.StartsWith(article))
                    text = text.Substring(article.Length);
            }
            return string.IsNullOrEmpty(text) ? "New cybersecurity task" : text;
        }

        private string ExtractReminderFromInput(string input)
        {
            string[] removePhrases = { "remind me", "set reminder", "reminder", "set a reminder", "to ", "a " };
            string text = input;
            foreach (var phrase in removePhrases)
            {
                if (text.Contains(phrase))
                    text = text.Replace(phrase, "").Trim();
            }

            string reminderDate = "tomorrow";
            if (text.Contains("tomorrow"))
            {
                reminderDate = DateTime.Now.AddDays(1).ToShortDateString();
                text = text.Replace("tomorrow", "").Trim();
            }
            else if (text.Contains("today"))
            {
                reminderDate = DateTime.Now.ToShortDateString();
                text = text.Replace("today", "").Trim();
            }
            else if (text.Contains("next week"))
            {
                reminderDate = DateTime.Now.AddDays(7).ToShortDateString();
                text = text.Replace("next week", "").Trim();
            }
            else if (text.Contains("in "))
            {
                var parts = text.Split(' ');
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i] == "in" && i + 1 < parts.Length)
                    {
                        if (int.TryParse(parts[i + 1], out int days))
                        {
                            reminderDate = DateTime.Now.AddDays(days).ToShortDateString();
                            text = text.Replace($"in {days} days", "").Trim();
                        }
                    }
                }
            }

            string reminderText = string.IsNullOrEmpty(text) ? "Cybersecurity reminder" : text;
            SharedActivityLog.Instance.AddEntry($"Reminder set: '{reminderText}' on {reminderDate}", "Reminder");
            return $"Reminder set for '{reminderText}' on {reminderDate}.";
        }

        private bool IsTaskRelatedCommand(string input)
        {
            string[] taskKeywords = { "add task", "new task", "create task" };
            foreach (var keyword in taskKeywords)
            {
                if (input.Contains(keyword))
                    return true;
            }
            return false;
        }

        private bool IsReminderRelatedCommand(string input)
        {
            string[] reminderKeywords = { "remind me", "set reminder" };
            foreach (var keyword in reminderKeywords)
            {
                if (input.Contains(keyword))
                    return true;
            }
            return false;
        }

        private string ProcessNLPCommand(string input)
        {
            // Check for log requests
            if (input.Contains("show log") || input.Contains("log") || input.Contains("history") || input.Contains("done for me"))
                return HandleActivityLogRequest();

            // Check for quiz
            if (input.Contains("quiz") || input.Contains("game") || input.Contains("play"))
                return "🎮 To start the cybersecurity quiz, please open the Quiz Mini-Game from the main menu!";

            // Check for specific cybersecurity topics - provide useful information
            if (input.Contains("password") || input.Contains("passwords"))
            {
                SharedActivityLog.Instance.AddEntry($"NLP: Password topic requested", "NLP");
                return "🔐 **Password Security:**\n" +
                       "• Use at least 12 characters with mixed case, numbers, and symbols\n" +
                       "• Never reuse passwords across different accounts\n" +
                       "• Use a password manager like Bitwarden or LastPass\n" +
                       "• Enable 2FA (Two-Factor Authentication) wherever possible\n" +
                       "• Change passwords immediately if you suspect a breach\n\n" +
                       "Type '7' to enter enhanced chat for more details!";
            }

            if (input.Contains("phishing"))
            {
                SharedActivityLog.Instance.AddEntry($"NLP: Phishing topic requested", "NLP");
                return "🎣 **Phishing Awareness:**\n" +
                       "• Check sender email addresses carefully - look for misspellings\n" +
                       "• Hover over links before clicking to see the actual URL\n" +
                       "• Never provide personal info via email or suspicious sites\n" +
                       "• Look for poor grammar or urgent requests - these are red flags\n" +
                       "• Use email filters and report suspicious messages\n\n" +
                       "Type '7' to enter enhanced chat for more details!";
            }

            if (input.Contains("vpn"))
            {
                SharedActivityLog.Instance.AddEntry($"NLP: VPN topic requested", "NLP");
                return "🌐 **VPN (Virtual Private Network):**\n" +
                       "• Encrypts your internet traffic and hides your IP address\n" +
                       "• Essential for public Wi-Fi security\n" +
                       "• Choose a trusted provider with a no-logs policy\n" +
                       "• Look for features like kill switch and DNS leak protection\n" +
                       "• Avoid free VPNs - they often sell your data\n\n" +
                       "Type '7' to enter enhanced chat for more details!";
            }

            if (input.Contains("malware") || input.Contains("virus"))
            {
                SharedActivityLog.Instance.AddEntry($"NLP: Malware topic requested", "NLP");
                return "🛡️ **Malware Protection:**\n" +
                       "• Install reputable antivirus software and keep it updated\n" +
                       "• Run regular system scans\n" +
                       "• Avoid opening attachments or links from unknown sources\n" +
                       "• Keep your operating system and software updated\n" +
                       "• Use ad-blockers and be cautious of pop-ups\n\n" +
                       "Type '7' to enter enhanced chat for more details!";
            }

            if (input.Contains("2fa") || input.Contains("two factor"))
            {
                SharedActivityLog.Instance.AddEntry($"NLP: 2FA topic requested", "NLP");
                return "🔑 **Two-Factor Authentication (2FA):**\n" +
                       "• Adds an extra security layer beyond just passwords\n" +
                       "• Uses something you know (password) and something you have (phone)\n" +
                       "• Use authenticator apps (Google Authenticator, Authy) over SMS\n" +
                       "• Enable 2FA on all important accounts (email, banking, social media)\n" +
                       "• Some services offer hardware tokens (YubiKey) for maximum security\n\n" +
                       "Type '7' to enter enhanced chat for more details!";
            }

            // General cybersecurity interest
            if (ContainsAny(input, new[] { "security", "cyber", "hack", "breach", "protect", "online", "safe" }))
            {
                SharedActivityLog.Instance.AddEntry($"NLP: General cybersecurity topic detected", "NLP");
                return "🔐 **Cybersecurity Essentials:**\n" +
                       "Here are key areas to focus on:\n" +
                       "• **Passwords** - Use strong, unique passwords for every account\n" +
                       "• **Phishing** - Learn to spot and avoid scams\n" +
                       "• **VPN** - Protect your privacy, especially on public Wi-Fi\n" +
                       "• **2FA** - Add an extra layer of security to your accounts\n" +
                       "• **Updates** - Keep all software and devices updated\n\n" +
                       "Type a specific topic above or type '7' for detailed chat!";
            }

            // Help menu
            if (input.Contains("help") || input.Contains("what can you do") || input.Contains("menu") || input.Contains("options"))
            {
                SharedActivityLog.Instance.AddEntry($"NLP: Help menu requested", "NLP");
                return "🤖 **I can help you with:**\n" +
                       "• Password security tips\n" +
                       "• Phishing awareness and detection\n" +
                       "• Safe browsing practices\n" +
                       "• VPN recommendations and setup\n" +
                       "• Malware protection\n" +
                       "• 2FA setup and best practices\n" +
                       "• Activity log ('show activity log')\n" +
                       "• Cybersecurity quiz (open from main menu)\n" +
                       "• Task management (open from main menu)\n\n" +
                       "Type '7' for a detailed conversation about any topic!";
            }

            return null;
        }

        private bool ContainsAny(string input, string[] keywords)
        {
            foreach (var keyword in keywords)
            {
                if (input.Contains(keyword))
                    return true;
            }
            return false;
        }

        // ============ TOPIC HANDLERS ============
        private string HandlePasswordTopic()
        {
            SharedActivityLog.Instance.AddEntry("Viewed password security tips", "Chat");
            return "🔐 **Password Tips:**\n" +
                   "• Use at least 12 characters mixing letters, numbers, and symbols\n" +
                   "• Avoid dictionary words and personal info\n" +
                   "• Use a password manager\n" +
                   "• Change passwords regularly\n" +
                   "• Enable 2FA for critical accounts\n\n" +
                   "I hope I have helped you with your password security request. You can now choose another topic to explore.";
        }

        private string HandlePhishingTopic()
        {
            SharedActivityLog.Instance.AddEntry("Viewed phishing awareness tips", "Chat");
            return "🎣 **Phishing Awareness:**\n" +
                   "• Check sender address before clicking\n" +
                   "• Hover over links before opening them\n" +
                   "• Avoid urgent requests or too-good-to-be-true offers\n" +
                   "• Never provide personal info via email\n" +
                   "• Use anti-phishing filters\n\n" +
                   "I hope I have helped you with your phishing awareness request. You can now choose another topic to explore.";
        }

        private string HandleSafeBrowsingTopic()
        {
            SharedActivityLog.Instance.AddEntry("Viewed safe browsing tips", "Chat");
            return "🌐 **Safe Browsing Tips:**\n" +
                   "• Use secure HTTPS connections\n" +
                   "• Update browser and extensions regularly\n" +
                   "• Avoid suspicious sites and downloads\n" +
                   "• Disable autofill for sensitive fields\n" +
                   "• Use a VPN on public Wi-Fi\n\n" +
                   "I hope I have helped you with your safe browsing request. You can now choose another topic to explore.";
        }

        private string HandleGeneralSecurityTopic()
        {
            SharedActivityLog.Instance.AddEntry("Viewed general security tips", "Chat");
            return "💡 **Cybersecurity Essentials:**\n" +
                   "• Unique password for each site\n" +
                   "• 2FA everywhere\n" +
                   "• Regular updates\n" +
                   "• Backups using 3-2-1 rule\n" +
                   "• Beware of social engineering\n\n" +
                   "I hope I have helped you with your cybersecurity essentials request. You can now choose another topic to explore.";
        }

        private string HandleEmergencyTopic()
        {
            SharedActivityLog.Instance.AddEntry("Viewed emergency protocol", "Chat");
            return "🆘 **Emergency Protocol:**\n" +
                   "1. Disconnect from the internet immediately\n" +
                   "2. Change passwords from a secure device\n" +
                   "3. Contact your IT or security team\n" +
                   "4. Monitor accounts for suspicious activity\n" +
                   "5. Consider freezing your credit report\n\n" +
                   "I hope I have helped you with your emergency response request. You can now choose another topic to explore.";
        }

        private string StartEnhancedChat()
        {
            inEnhancedChat = true;
            SharedActivityLog.Instance.AddEntry("Started enhanced chat mode", "Chat");
            return $"🤖 Let's dive deeper, {userName}! Tell me what's on your mind, and I'll try to help. You can say things like:\n" +
                   "• 'I'm worried about phishing'\n" +
                   "• 'How do I stay safe online?'\n" +
                   "• 'Tell me about VPNs'\n" +
                   "• 'What is social engineering?'\n\n" +
                   "Type 'exit' anytime to return to the menu.";
        }

        private string GetFarewellMessage()
        {
            string[] farewells = {
                "Remember to always think before you click!",
                "Stay vigilant against online threats!",
                "Your security is worth the extra effort!",
                "Keep your digital life safe and secure!"
            };
            var rnd = new Random();
            SharedActivityLog.Instance.AddEntry($"User said goodbye", "Chat");
            return $"👋 Goodbye, {userName}! {farewells[rnd.Next(farewells.Length)]}";
        }

        private string HandleEnhancedConversation(string input)
        {
            // Check for specific topics in enhanced chat
            if (input.Contains("phishing"))
            {
                pendingTopic = "phishing";
                return "⚠️ **Phishing attacks** can trick even tech-savvy users. Watch for misspelled URLs, strange attachments, or unexpected messages. Always verify before clicking. Would you like detailed tips to identify them better? (Reply 'yes' or 'sure')";
            }
            else if (input.Contains("password") || input.Contains("passwords"))
            {
                pendingTopic = "password";
                return "🔐 **Passwords** are your first defense. Use passphrases like 'PurpleGiraffe$Dance2025'. Never reuse passwords across accounts. Would you like a list of best practices? (Reply 'yes' or 'sure')";
            }
            else if (input.Contains("vpn"))
            {
                pendingTopic = "vpn";
                return "🌐 **VPNs** protect your internet traffic, especially on public Wi-Fi. Choose one with a no-logs policy and fast servers. Need help picking one? (Reply 'yes' or 'sure')";
            }
            else if (input.Contains("malware") || input.Contains("virus"))
            {
                pendingTopic = "malware";
                return "🦠 **Malware** can enter through email, downloads, or ads. Run antivirus tools weekly and avoid shady links. Want a checklist to stay protected? (Reply 'yes' or 'sure')";
            }
            else if (input.Contains("hacked"))
            {
                pendingTopic = "hacked";
                return "🚨 If you think you've been **hacked**, change your passwords immediately, enable 2FA, and scan your device. Do you need recovery steps? (Reply 'yes' or 'sure')";
            }
            else if (input.Contains("social media"))
            {
                pendingTopic = "social media";
                return "📱 **Social media** can leak personal data. Use privacy settings, avoid location tags, and don't overshare. Want a guide to secure your accounts? (Reply 'yes' or 'sure')";
            }
            else if (input.Contains("wifi") || input.Contains("network"))
            {
                pendingTopic = "wifi";
                return "📡 **Home networks** must be secured. Change default router passwords, use WPA3 encryption, and set a guest network. Need full setup steps? (Reply 'yes' or 'sure')";
            }
            else if (input.Contains("2fa") || input.Contains("two factor"))
            {
                pendingTopic = "2fa";
                return "🔑 **Two-Factor Authentication** adds an extra security layer. Use authenticator apps instead of SMS when possible. Want setup guides? (Reply 'yes' or 'sure')";
            }
            else if (input.Contains("how are you"))
            {
                return "🤖 I'm functioning securely and ready to assist! What's on your mind today?";
            }
            else if (input.Contains("help") || input.Contains("what can you do"))
            {
                return "🤖 In enhanced chat, you can ask me about:\n" +
                       "• Phishing attacks\n" +
                       "• Password security\n" +
                       "• VPNs\n" +
                       "• Malware protection\n" +
                       "• Hacked accounts\n" +
                       "• Social media privacy\n" +
                       "• Wi-Fi security\n" +
                       "• 2FA setup\n\n" +
                       "Just ask about any of these topics!";
            }

            // If no specific topic matched, suggest options
            return "🤔 That's interesting. I can help you with:\n" +
                   "• Phishing\n• Passwords\n• VPNs\n• Malware\n• Hacked accounts\n" +
                   "• Social media privacy\n• Wi-Fi security\n• 2FA\n\n" +
                   "What would you like to learn about? (Type 'exit' to return to menu)";
        }

        private string TriggerTopicExpansion(string topic)
        {
            return topic switch
            {
                "phishing" => "🎯 **Phishing Prevention Tips:**\n" +
                               "• Don't trust urgent emails asking for sensitive info\n" +
                               "• Hover over links before clicking to see the real URL\n" +
                               "• Verify the sender address (look for misspellings)\n" +
                               "• Look for poor grammar or strange logos\n" +
                               "• Use email filters and antivirus tools\n" +
                               "• When in doubt, contact the company directly using known contact info",

                "password" => "💡 **Password Best Practices:**\n" +
                               "• Use a passphrase with 12+ characters (e.g., 'PurpleCats$Dance2025')\n" +
                               "• Never reuse the same password across accounts\n" +
                               "• Use a password manager (Bitwarden, 1Password, LastPass)\n" +
                               "• Enable 2FA for every account that offers it\n" +
                               "• Change passwords if you suspect any breach\n" +
                               "• Avoid using personal info (birthdays, pet names)",

                "vpn" => "🛡️ **VPN Safety Tips:**\n" +
                          "• Choose trusted providers with a no-logs policy\n" +
                          "• Always use VPN on public Wi-Fi networks\n" +
                          "• Avoid free VPNs (they often sell your data)\n" +
                          "• Check for DNS/IP leak protection\n" +
                          "• Enable kill switch feature for security\n" +
                          "• Popular options: NordVPN, ExpressVPN, ProtonVPN",

                "malware" => "🛠️ **Malware Prevention Checklist:**\n" +
                               "• Run antivirus/anti-malware scans weekly\n" +
                               "• Never open unknown email attachments\n" +
                               "• Don't download software from untrusted sites\n" +
                               "• Keep OS and applications auto-updated\n" +
                               "• Use ad-blockers in your browser\n" +
                               "• Back up important files regularly",

                "hacked" => "🚨 **Hacked Account Response:**\n" +
                             "1. Change all passwords immediately (use a different device)\n" +
                             "2. Enable 2FA on all accounts\n" +
                             "3. Run a full antivirus/malware scan\n" +
                             "4. Review recent login history and active sessions\n" +
                             "5. Log out of all devices\n" +
                             "6. Notify affected services and contacts\n" +
                             "7. Consider credit freeze if financial info was exposed",

                "social media" => "📲 **Social Media Protection Guide:**\n" +
                                    "• Set all profiles to private\n" +
                                    "• Avoid sharing location or travel plans\n" +
                                    "• Don't overshare personal info (address, phone, birthdate)\n" +
                                    "• Use strong, unique passwords for each platform\n" +
                                    "• Disable location tags on posts\n" +
                                    "• Review privacy settings monthly\n" +
                                    "• Be cautious of friend requests from strangers",

                "wifi" => "📶 **WiFi Security Setup:**\n" +
                            "• Rename default network name (SSID)\n" +
                            "• Set a strong WiFi password (WPA3/WPA2)\n" +
                            "• Enable WPA3 encryption if available\n" +
                            "• Create a separate guest network\n" +
                            "• Update router firmware regularly\n" +
                            "• Disable WPS and remote management\n" +
                            "• Use VPN for sensitive activities",

                "2fa" => "🔑 **2FA Setup Guide:**\n" +
                           "• Use authenticator apps (Google Authenticator, Authy)\n" +
                           "• Avoid SMS-based 2FA when possible (less secure)\n" +
                           "• Enable 2FA on: Email, Banking, Social Media, Cloud Storage\n" +
                           "• Backup your 2FA recovery codes securely\n" +
                           "• Consider hardware tokens (YubiKey) for extra security",

                _ => "📘 **General Security Tip:**\n" +
                       "Always stay informed about online threats. The best defense is awareness!\n" +
                       "• Use strong, unique passwords\n" +
                       "• Enable 2FA everywhere\n" +
                       "• Keep software updated\n" +
                       "• Think before you click\n" +
                       "• Protect your personal information"
            };
        }

        private void BackgroundVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundVideo.Position = TimeSpan.Zero;
            BackgroundVideo.Play();
        }
    }
}