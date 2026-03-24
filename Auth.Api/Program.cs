using Amazon.Runtime;
using Amazon.S3;
using Auth.Api.Configurations;
using Auth.Api.Extensions;
using Auth.Api.Middlewares;
using Auth.DataAccess.AppDbContexts;
using Auth.Service.Helpers;
using Auth.Service.Interfaces;
using Auth.Service.Seeders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ----------------- Logging ------------------
builder.Host.UseSerilog((ctx, services, config) =>
{
    config.ReadFrom.Configuration(ctx.Configuration) 
          .Enrich.FromLogContext()
          .WriteTo.Console();
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddHealthChecks();

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
));

// ----------------- AWS S3 -------------------
var awsOptions = builder.Configuration.GetSection("AWS");
builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var config = new AmazonS3Config
    {
        ServiceURL = awsOptions["ServiceURL"],
        ForcePathStyle = true,
        AuthenticationRegion = awsOptions["Region"]
    };
    var credentials = new BasicAWSCredentials(awsOptions["AccessKey"], awsOptions["SecretKey"]);
    return new AmazonS3Client(credentials, config);
});

// ----------------- DB Context ----------------
builder.Services.Configure<HostOptions>(options =>
{
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ----------------- Services ------------------
builder.Services.AddServices();
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Program).Assembly));
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddJwtService(builder.Configuration);
builder.Services.AddSwaggerService();
builder.Services.AddMemoryCache();
builder.Services.AddLogging();

// ----------------- Controllers ----------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
EnvironmentHelper.WebRootPath = builder.Environment.WebRootPath;
builder.Services.AddHttpContextAccessor();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi  
builder.Services.AddOpenApi();

var app = builder.Build();

// ----------------- Apply Migrations & Seeding ----------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<AppDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        var userService = services.GetRequiredService<IUserService>();
        var permissionService = services.GetRequiredService<IPermissionService>();
        var roleService = services.GetRequiredService<IRoleService>();

        logger.LogInformation("🔄 Starting database migration and seeding...");

        db.Database.Migrate();
        await DbSeeder.SeedAsync(db, userService, permissionService, roleService, logger);

        logger.LogInformation("✅ Databasze migration and seeding completed.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();

        logger.LogError(ex, "❌ Database migration and seeding failed.");
    }
}

// ----------------- Middleware Configuration ----------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Assign global HttpContextAccessor
if (app.Services.GetService<IHttpContextAccessor>() != null)
    HttpContextHelper.Accessor = app.Services.GetRequiredService<IHttpContextAccessor>();

// ----------------- HTTP Pipeline ----------------
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapStaticAssets();

app.UseStaticFiles();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// 🧩 Custom middlewares - ORDER IS IMPORTANT!
app.UseMiddleware<GlobalExceptionMiddleware>(); // First
app.UseMiddleware<DatabaseLoggingMiddleware>();
app.UseMiddleware<TokenValidationMidlleware>();


app.UseCors("AllowAll");
app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
