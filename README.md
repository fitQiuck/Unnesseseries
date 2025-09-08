<h1 align="center">📚 EnglishUp</h1>  

<p align="center">
  <em>A modern English learning platform designed to make studying fun, effective, and interactive — like Duolingo, but better!</em>
</p>  

<p align="center">
  <img src="https://img.shields.io/badge/.NET-9.0-blueviolet?style=for-the-badge&logo=dotnet" alt=".NET 9" />
  <img src="https://img.shields.io/badge/PostgreSQL-DB-blue?style=for-the-badge&logo=postgresql" alt="PostgreSQL" />
  <img src="https://img.shields.io/badge/Docker-ready-green?style=for-the-badge&logo=docker" alt="Docker" />
</p>  

---

## 🚀 Features  

✅ **User Accounts & Authentication** — secure login with roles & permissions.  
✅ **Courses & Lessons** — structured learning path with lesson parts (grammar, vocabulary, listening).  
✅ **Daily Challenges & Streaks** — keep learners motivated with progress rewards.  
✅ **Homework & Assignments** — tasks & submissions for deeper practice.  
✅ **Mock Tests (IELTS-style)** — Listening, Reading, Writing, Speaking with scoring.  
✅ **Subscriptions & Points** — free & premium plans, gamified achievements.  
✅ **Progress Tracking** — streak logs, graphs, dashboards for learners.  

---

## 🛠️ Tech Stack  

<table>
<tr>
<td><b>Backend</b></td><td>C# .NET 9, ASP.NET Core Web API</td>
</tr>
<tr>
<td><b>Database</b></td><td>PostgreSQL + EF Core</td>
</tr>
<tr>
<td><b>Authentication</b></td><td>JWT Tokens</td>
</tr>
<tr>
<td><b>DevOps</b></td><td>Docker, GitLab CI/CD</td>
</tr>
<tr>
<td><b>Frontend (Planned)</b></td><td>React / Next.js or Angular</td>
</tr>
</table>  

---

## 📂 Project Structure  

```bash
EnglishUp/
│
├── Auth.API              # Main entry point for authentication
├── Auth.Domain           # Domain models (User, Role, Token, etc.)
├── Auth.Repository       # EF Core repositories & migrations
├── Auth.Service          # Services, DTOs, helpers, exceptions
├── CourseService/        # Handles courses and lessons
├── HomeworkService/      # Manages user homework & submissions
├── MockTestService/      # IELTS-style mock test logic
├── StreakService/        # Streaks & streak logs
├── SubscriptionService/  # Plans & subscriptions
```

---

## 📊 Example Entities

👤 User → profile, progress, subscriptions

📘 Course → lessons, lesson parts, levels (Beginner → Advanced)

📝 Homework → tasks & submissions

🧪 MockTest → results (Listening, Reading, Writing, Speaking)

🔥 Streak → daily learning activity tracking

💳 Subscription → free & premium tiers

## 🤝 Contributing

We welcome contributions! 🎉

1. Create a new branch:
```
git checkout -b feature-name
```
2. Commit your changes:
```
git commit -m "Added new feature"
```
3. Push and open a Pull Request

<p align="center"> <img src="https://img.shields.io/badge/PRs-welcome-brightgreen?style=for-the-badge&logo=github" alt="PRs Welcome" /> </p>

## 📅 Roadmap

 🚀 Launch MVP version with core features

 🤖 Add AI-powered speaking & writing evaluation

 🌍 Expand to TOEFL & other exams

 📱 Mobile app (iOS & Android)|

Here's all domain classes of the project, including their enums :
<img width="1177" height="2897" alt="Untitled" src="https://github.com/user-attachments/assets/f2161bee-03f3-4afc-b8f3-130656fd4901" />
