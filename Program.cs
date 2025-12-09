using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SberVolunteerAPI.Models;
using SberVolunteerAPI.Services;
using System;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var cs = $"Server={Environment.GetEnvironmentVariable("DB_HOST")};" +
         $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
         $"Database={Environment.GetEnvironmentVariable("DB_DATABASE")};" +
         $"User={Environment.GetEnvironmentVariable("DB_USER")};" +
         $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

builder.Services.AddDbContext<SberVolunteerContext>(options =>
    options.UseMySql(cs, ServerVersion.AutoDetect(cs)));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JWTService>();
builder.Services.Configure<AuthSettings>(
    builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddScoped<VolunteerService>();
builder.Services.AddScoped<OrganiserService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "¬ведите токен вида: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// JWT auth

builder.Services
    .AddAuthentication(options =>
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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SberVolunteerContext>();
    db.Database.Migrate();
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();   // << об€зательно перед Authorization!
app.UseAuthorization();

app.MapControllers();

app.Run();
