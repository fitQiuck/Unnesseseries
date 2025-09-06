using Auth.DataAccess.Interface;
using Auth.DataAccess.Repository;
using Auth.Service.Interfaces;
using Auth.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Auth.Api.Extensions;

public static class ServiceExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        // Repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Services
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<ILogService, LogService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<ICourseService, CourseService>();
        services.AddTransient<ICourseLevelService, CourseLevelService>();
        services.AddTransient<IDaliyChallengeService, DailyChallengeService>();
        services.AddTransient<IHomeworkService, HomeworkService>();
        services.AddTransient<ILessonService, LessonService>();
        services.AddTransient<IMockTestService, MockTestService>();
        services.AddTransient<IPermissionService, PermissionService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IStreakLogService, StreakLogService>();
        services.AddTransient<IStreakService, StreakService>();
        services.AddTransient<ISubscriptionService, SubscriptionService>();
        services.AddTransient<ISubscriptionPlanService, SubscriptionPlanService>();
        services.AddTransient<ITestResultService, TestResultService>();
        services.AddTransient<IMockTestQuestionService, MockTestQuestionService>();
        services.AddTransient<IUserCourseService, UserCourseService>();
        services.AddTransient<IUserHomeworkService, UserHomeworkService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserChallengeService, UserChallengeService>();
        services.AddTransient<ICourseCommentService, CourseCommentService>();
        services.AddTransient<IFileService, FileService>();

    }



    public static void AddJwtService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var Key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Key)
            };
        });
        services.AddAuthorization();
        services.AddControllers();
    }

    public static void AddSwaggerService(this IServiceCollection services)
    {
        services.AddSwaggerGen(p =>
        {
            p.ResolveConflictingActions(ad => ad.First());
            p.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
            });

            p.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
        });
    }
}
