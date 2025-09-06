using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.ErrorsDetails;
using Auth.Domain.Entities.Gamification;
using Auth.Domain.Entities.Homeworks;
using Auth.Domain.Entities.Logs;
using Auth.Domain.Entities.Permissions;
using Auth.Domain.Entities.Roles;
using Auth.Domain.Entities.Subscriptions;
using Auth.Domain.Entities.Tests;
using Auth.Domain.Entities.Tokens;
using Auth.Domain.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace Auth.DataAccess.AppDbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // All related to User Management
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserChallenge> UserChallenges { get; set; }

    //Tokens
    public virtual DbSet<Token> Tokens { get; set; }
    //ErrorDetails
    public virtual DbSet<ErrorDetails> ErrorDetails { get; set; }

    //Logs
    public virtual DbSet<Log> Logs { get; set; }

    //All related to Courses
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<CourseLevel> CourseLevels { get; set; }
    public virtual DbSet<CourseComment> CourseComments { get; set; }
    public virtual DbSet<UserCourse> UserCourses { get; set; }
    public virtual DbSet<Lesson> Lessons { get; set; }

    // All related to Permissions and Roles
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Permission> Permissions { get; set; }

    // All related to Gamification
    public virtual DbSet<DailyChallengge> DailyChallenges { get; set; }
    public virtual DbSet<Streak> Streaks { get; set; }
    public virtual DbSet<StreakLog> StreakLogs { get; set; }

    // All related to Homeworks
    public virtual DbSet<Homework> Homeworks { get; set; }
    public virtual DbSet<UserHomework> UserHomeworks { get; set; }

    // All related to Tests
    public virtual DbSet<MockTest> MockTests { get; set; }
    public virtual DbSet<MockTestQuestion> MockTestQuestions { get; set; }
    public virtual DbSet<TestResult> TestResults { get; set; }

    // All related to Subscriptions
    public virtual DbSet<Subscription> Subscriptions { get; set; }
    public virtual DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

}
