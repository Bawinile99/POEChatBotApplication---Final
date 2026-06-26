# 🛡️ Cybersecurity Awareness Chatbot

## A Comprehensive WPF Desktop Application for Cybersecurity Education

---

## 📋 Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Prerequisites](#prerequisites)
- [Installation Guide](#installation-guide)
- [Database Setup](#database-setup)
- [Project Structure](#project-structure)
- [Usage Guide](#usage-guide)
- [Feature Checklist](#feature-checklist)
- [Troubleshooting](#troubleshooting)
- [Video Presentation](#video-presentation)
- [License](#license)
- [Acknowledgments](#acknowledgments)

---

## 📖 Overview

The **Cybersecurity Awareness Chatbot** is a fully-featured WPF desktop application designed to educate users about cybersecurity best practices through interactive engagement. The application combines a smart chatbot, task management system, interactive quiz, and activity logging to create a comprehensive learning experience.

### Key Features at a Glance:
- 🤖 **Smart Chat Assistant** with NLP simulation
- 📋 **Task Manager** with MySQL database integration
- 🎮 **Interactive Cybersecurity Quiz** with 15+ questions
- 📊 **Activity Log** with timestamps and filtering
- 🎨 **Modern Cyber-Themed UI** with animations

---

## ✨ Features

### 1. 🤖 Smart Chat Assistant
- **Cybersecurity Tips**: Comprehensive information on passwords, phishing, safe browsing, VPNs, malware, and more
- **NLP Simulation**: Keyword detection and flexible command recognition
- **Sentiment Analysis**: Detects user mood and responds appropriately
- **Enhanced Chat Mode**: Deep dive conversations on security topics
- **Emergency Protocol**: Step-by-step guidance for security incidents

**Example Commands:**
```
User: "passwords" → CyberBot: Detailed password security tips
User: "phishing" → CyberBot: Phishing awareness information
User: "show activity log" → CyberBot: Displays recent actions
User: "help" → CyberBot: Shows available commands
```

### 2. 📋 Task Assistant with Database
- **MySQL Integration**: Persistent storage of cybersecurity tasks
- **Full CRUD Operations**: Add, edit, delete, and complete tasks
- **Reminders**: Set reminders with specific dates for each task
- **Task Tracking**: View all tasks with completion status
- **Real-time Updates**: Changes instantly reflect in the database

**Task Management Features:**
- Add task with title, description, and due date
- Edit existing tasks
- Delete tasks with confirmation
- Mark tasks as complete/incomplete
- View all tasks organized by status

### 3. 🎮 Cybersecurity Quiz Game
- **15+ Comprehensive Questions**: Covers phishing, passwords, malware, social engineering, VPNs, and more
- **Mixed Question Formats**: Multiple-choice and true/false questions
- **Immediate Feedback**: Shows correct answer with detailed explanation
- **Score Tracking**: Real-time scoring with progress bar
- **Final Evaluation**: Motivational feedback based on performance

**Quiz Topics Covered:**
- Phishing attacks and prevention
- Password security best practices
- Malware types and protection
- Social engineering awareness
- VPN usage and security
- Two-Factor Authentication (2FA)
- Safe browsing practices
- Software updates importance

### 4. 📊 Activity Log
- **Automatic Tracking**: Logs all significant user actions
- **Categories**: Task, Quiz, Chat, Reminder, NLP, Log
- **Timestamps**: Each entry includes date and time
- **Filtering**: View logs by category
- **Show More**: Full history viewing capability
- **Clear Log**: Option to clear all entries

**Logged Activities:**
- Task additions, edits, deletions, completions
- Quiz starts, answers, completions
- Chat interactions and topics
- Reminder settings
- NLP interpretations
- System events

---

## 🛠️ Technology Stack

| Component | Technology |
|-----------|------------|
| **Framework** | .NET 6.0 WPF |
| **UI** | XAML with custom styles and animations |
| **Database** | MySQL 8.0+ (via XAMPP) |
| **Architecture** | MVVM Pattern |
| **Programming Language** | C# |
| **IDE** | Visual Studio 2022 |
| **Version Control** | Git / GitHub |

### NuGet Packages Used:
```
- MySql.Data (8.0.x) - Database connectivity
```

---

## 📦 Prerequisites

### Required Software:
- **Windows 10/11** (64-bit recommended)
- **Visual Studio 2022** (Community or higher)
  - Include: .NET desktop development workload
- **XAMPP** (or MySQL Server 8.0+)
- **.NET 6.0 SDK** (Included with Visual Studio 2022)

### Recommended:
- **4GB+ RAM**
- **2GB+ free disk space**

---

## 🚀 Installation Guide

### Step 1: Clone the Repository
```bash
git clone https://github.com/yourusername/cybersecurity-chatbot.git
cd cybersecurity-chatbot
```

### Step 2: Open in Visual Studio 2022
1. Launch Visual Studio 2022
2. Click **"Open a project or solution"**
3. Navigate to the cloned folder
4. Select the `.sln` file
5. Click **"Open"**

### Step 3: Install NuGet Packages
1. In Visual Studio, go to **Tools** → **NuGet Package Manager** → **Package Manager Console**
2. Run this command:
```powershell
Install-Package MySql.Data
```

### Step 4: Set Up XAMPP
1. Launch **XAMPP Control Panel**
2. Click **"Start"** next to **MySQL**
3. Ensure the status shows **"Running"** (green)

### Step 5: Set Up the Database
1. Open your browser and go to: http://localhost/phpmyadmin/
2. Click **"New"** on the left sidebar
3. Create database:
```
Database name: cyberbot_db
Collation: utf8mb4_general_ci
```
4. Click **"Create"**
5. Click the **"SQL"** tab
6. Copy and paste the SQL script (see Database Setup section below)
7. Click **"Go"**

### Step 6: Update Connection String
In `TaskView.xaml.cs`, update the connection string:
```csharp
private string connectionString = "Server=localhost;Database=cyberbot_db;Uid=root;Pwd=;";
```
> **Note**: If you set a password for MySQL, replace the empty string with your password.

### Step 7: Build and Run
1. Press **Ctrl+Shift+B** to build the solution
2. Press **F5** to run the application
3. The splash screen should appear, followed by the main window

---

## 📊 Database Setup

### Complete SQL Script
Run this script in phpMyAdmin to set up your database:

```sql
-- ============================================
-- CYBERSECURITY CHATBOT DATABASE
-- ============================================

-- Create the database
CREATE DATABASE IF NOT EXISTS cyberbot_db;
USE cyberbot_db;

-- Create tasks table
CREATE TABLE tasks (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT,
    ReminderDate DATETIME,
    IsCompleted BOOLEAN DEFAULT FALSE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Insert sample cybersecurity tasks
INSERT INTO tasks (Title, Description, ReminderDate) VALUES
('Enable Two-Factor Authentication', 
 'Enable 2FA for all important accounts including email and social media', 
 DATE_ADD(NOW(), INTERVAL 5 DAY)),

('Review Privacy Settings', 
 'Check and update privacy settings on social media platforms', 
 DATE_ADD(NOW(), INTERVAL 7 DAY)),

('Update Passwords', 
 'Update passwords for email, banking, and social media accounts', 
 DATE_ADD(NOW(), INTERVAL 3 DAY)),

('Install Antivirus Software', 
 'Install and configure antivirus software on all devices', 
 DATE_ADD(NOW(), INTERVAL 10 DAY)),

('Backup Important Files', 
 'Create backups of important files using 3-2-1 backup strategy', 
 DATE_ADD(NOW(), INTERVAL 14 DAY));

-- Insert completed tasks for testing
INSERT INTO tasks (Title, Description, ReminderDate, IsCompleted) VALUES
('Review Browser Extensions', 
 'Review and remove unnecessary browser extensions', 
 DATE_ADD(NOW(), INTERVAL 1 DAY), 
 TRUE),

('Enable Screen Lock', 
 'Enable screen lock on all devices with strong authentication', 
 DATE_ADD(NOW(), INTERVAL 2 DAY), 
 TRUE);

-- View all tasks
SELECT * FROM tasks;

-- Show statistics
SELECT 
    COUNT(*) AS TotalTasks,
    SUM(CASE WHEN IsCompleted = 0 THEN 1 ELSE 0 END) AS PendingTasks,
    SUM(CASE WHEN IsCompleted = 1 THEN 1 ELSE 0 END) AS CompletedTasks
FROM tasks;
```

---

## 📁 Project Structure

```
POEChatBotApplication/
│
├── Converters/
│   ├── BoolToAlignmentConverter.cs
│   └── BoolToBubbleColorConverter.cs
│
├── Models/
│   └── Task.cs
│
├── Views/
│   ├── MainWindow.xaml
│   ├── MainWindow.xaml.cs
│   ├── ChatView.xaml
│   ├── ChatView.xaml.cs
│   ├── TaskView.xaml
│   ├── TaskView.xaml.cs
│   ├── QuizView.xaml
│   ├── QuizView.xaml.cs
│   ├── ActivityLogView.xaml
│   ├── ActivityLogView.xaml.cs
│   ├── SplashScreen.xaml
│   └── SplashScreen.xaml.cs
│
├── Themes/
│   └── Generic.xaml
│
├── Images/
│   ├── MatrixDigitalRain.mp4
│   └── vvi.mp4
│
├── App.xaml
├── App.xaml.cs
├── README.md
└── setup_database.sql
```

---

## 🎮 Usage Guide

### 1. 🏠 Main Menu Navigation

The main window provides four navigation buttons:

| Button | Function | Description |
|--------|----------|-------------|
| 💬 **Chat** | Opens Chat Assistant | Interact with CyberBot, ask questions |
| 📋 **Tasks** | Opens Task Manager | Manage cybersecurity tasks |
| 🎮 **Quiz** | Opens Quiz Game | Test cybersecurity knowledge |
| 📊 **Activity Log** | Opens Activity Log | View all system activities |

### 2. 💬 Chat Assistant

#### Basic Commands:
```
"1" - Password security tips
"2" - Phishing awareness
"3" - Safe browsing practices
"4" - General cybersecurity tips
"5" - Emergency protocol
"6" - View activity log
"7" - Enhanced chat mode
"exit" or "goodbye" - End conversation
"help" - Show available commands
```

#### NLP Commands (Natural Language):
```
"passwords" - Get password security tips
"phishing" - Get phishing awareness info
"vpn" - Get VPN information
"malware" - Get malware protection tips
"2fa" or "two factor" - Get 2FA information
"show activity log" - View recent actions
"what have you done for me" - View activity log
"add task [title]" - Create a new task
"remind me [task] tomorrow" - Set a reminder
```

#### Enhanced Chat Mode:
Type **"7"** or **"enhanced"** or **"deep chat"** to enter detailed conversation mode. You can ask questions like:
- "I'm worried about phishing"
- "How do I stay safe online?"
- "Tell me about VPNs"
- "What is social engineering?"
- "How do I know if I've been hacked?"

### 3. 📋 Task Manager

#### Adding a Task:
1. Enter a **Title** (required)
2. Enter a **Description** (optional)
3. Select a **Due Date** (default: 7 days from now)
4. Click **"DEPLOY TASK"**

#### Editing a Task:
1. Select a task from the list
2. Click the **"✏"** (edit) button
3. Update the fields
4. Click **"DEPLOY TASK"** to save changes

#### Deleting a Task:
1. Select a task from the list
2. Click the **"✕"** (delete) button
3. Confirm deletion

#### Completing a Task:
1. Select a task from the list
2. Click the **"✓"** (complete) button
3. Task will be marked as completed

### 4. 🎮 Cybersecurity Quiz

#### Taking the Quiz:
1. Read the question carefully
2. Select an answer from the options
3. Click **"SUBMIT ANSWER"**
4. Immediate feedback will appear with explanation
5. Continue until all questions are answered

#### Quiz Feedback:
- **Correct**: Shows green "Correct!" with explanation
- **Incorrect**: Shows correct answer with explanation
- **Score**: Updated in real-time with progress bar
- **Final**: Motivational feedback based on score

### 5. 📊 Activity Log

#### Viewing Activities:
- All activities are automatically logged
- Click **"Activity Log"** to view
- Filter by category using the dropdown
- Click **"Show More"** for full history

#### Categories:
- **Task**: Task-related activities
- **Quiz**: Quiz interactions
- **Chat**: Chat conversations
- **Reminder**: Reminder settings
- **NLP**: NLP interpretations
- **Log**: System events

---

## ✅ Feature Checklist

### Task 1: Task Assistant with Reminders (GUI) ✅
- [x] MySQL database integration
- [x] Add tasks with title, description, and reminders
- [x] Edit existing tasks
- [x] Delete tasks with confirmation
- [x] Mark tasks as completed
- [x] Changes reflect in database
- [x] View all tasks with status

### Task 2: Cybersecurity Mini-Game (Quiz) ✅
- [x] 15+ cybersecurity questions
- [x] Mixed multiple-choice and true/false
- [x] One question at a time
- [x] Immediate feedback with explanations
- [x] Score tracking with progress bar
- [x] Final feedback based on performance

### Task 3: Natural Language Processing (NLP) Simulation ✅
- [x] Keyword detection for various commands
- [x] Flexible phrase recognition
- [x] Task and reminder extraction
- [x] Sentiment detection
- [x] Limited "I don't understand" responses
- [x] Enhanced chat mode for detailed conversations

### Task 4: Activity Log Feature ✅
- [x] Logs all significant actions
- [x] Timestamps on all entries
- [x] Display last 5-10 actions
- [x] "Show more" functionality
- [x] Category filtering
- [x] Clear log option

---

## 🔧 Troubleshooting

### Common Issues and Solutions

#### 1. MySQL Won't Start in XAMPP
**Solution:**
```
1. Open Task Manager (Ctrl+Shift+Esc)
2. Go to Services tab
3. Find any MySQL services and stop them
4. Delete C:\xampp\mysql\data\ib_logfile0 and ib_logfile1
5. Restart MySQL in XAMPP Control Panel
```

#### 2. Database Connection Failed
**Solution:**
```
1. Check if XAMPP MySQL is running (green icon)
2. Verify connection string in TaskView.xaml.cs
3. Test in phpMyAdmin: http://localhost/phpmyadmin/
4. Make sure database name is correct: cyberbot_db
```

#### 3. Application Won't Build
**Solution:**
```
1. Clean solution: Build → Clean Solution
2. Rebuild: Build → Rebuild Solution
3. Restart Visual Studio
4. Check for missing NuGet packages
5. Ensure all using statements are correct
```

#### 4. Video Background Not Playing
**Solution:**
```
1. Check file path in XAML: Source="images/vvi.mp4"
2. Ensure video files exist in the images folder
3. Set Build Action to "Content"
4. Set Copy to Output Directory to "Copy if newer"
```

---

## 🎥 Video Presentation

### YouTube Unlisted Link
[Insert your YouTube video link here]

### Presentation Guidelines:
- **Duration**: 5-10 minutes
- **Content**:
  - Introduction to the project
  - Demonstration of all features
  - Code walkthrough (key parts)
  - Database setup and integration
  - Testing and validation
- **Quality**: Clear voice-over, HD video
- **Privacy**: Set to "Unlisted"

---

## 📝 License

This project is created for **Educational Purposes** as part of the POE (Practical Outcome Evaluation) submission.

- **Author**: Bawinile Mahlangu


---

## 🙏 Acknowledgments

### Special Thanks To:
- **Lecturers and Instructors** - For guidance and support
- **Open Source Community** - For tools and libraries
- **Cybersecurity Professionals** - For best practices and insights

### Resources Used:
- **MySQL Documentation** - https://dev.mysql.com/doc/
- **Microsoft WPF Documentation** - https://docs.microsoft.com/wpf/
- **XAMPP** - https://www.apachefriends.org/
- **Cybersecurity Best Practices** - NIST, OWASP

---

## 📞 Contact


---

## 🔄 Version History

| Version | Date | Changes |
|---------|------|---------|
| v1.0.0 | June 2026 | Initial release |
| v1.1.0 | June 2026 | Added Activity Log feature |
| v1.2.0 | June 2026 | Final POE submission version |

---

## 📚 Additional Resources

### Recommended Reading:
1. "Cybersecurity for Beginners" - Raef Meeuwisse
2. NIST Cybersecurity Framework
3. OWASP Top 10 Security Risks

### Online Resources:
- **NIST**: https://www.nist.gov/cyberframework
- **OWASP**: https://owasp.org/
- **SANS Institute**: https://www.sans.org/

---

**Built with ❤️ for Cybersecurity Awareness**

**Bawinile Mahlangu** | June 2026

---

*This README was last updated on June 2026*
