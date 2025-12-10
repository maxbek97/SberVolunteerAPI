using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SberVolunteerAPI.Models;
using SberVolunteerAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Добавляем appsettings.Local.json (опционально)
builder.Configuration.AddJsonFile("appsettings_local.json", optional: true);

builder.Services.AddControllers();

// 1) Пытаемся взять строку подключения из переменных окружения (для Docker)
var dockerConnectionString = Environment.GetEnvironmentVariable("DB_HOST") != null
    ? $"Server={Environment.GetEnvironmentVariable("DB_HOST")};" +
      $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
      $"Database={Environment.GetEnvironmentVariable("DB_DATABASE")};" +
      $"User={Environment.GetEnvironmentVariable("DB_USER")};" +
      $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};"
    : null;

// 2) Если работаем локально → берем строку подключения из appsettings
var connectionString = dockerConnectionString
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<SberVolunteerContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JWTService>();
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["AuthSettings:SecretKey"]))
    };
});

var app = builder.Build();

// Авто-миграции
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SberVolunteerContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
