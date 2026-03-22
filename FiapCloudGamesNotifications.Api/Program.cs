using FiapCloudGamesNotifications.Api.Authorize;
using FiapCloudGamesNotifications.Api.Endpoints;
using FiapCloudGamesNotifications.Api.Extensions;
using FiapCloudGamesNotifications.Application.Middlewares;
using FiapCloudGamesNotifications.Application.Services;
using FiapCloudGamesNotifications.Application.Services.Interfaces;
using FiapCloudGamesNotifications.Domain.Repositories;
using FiapCloudGamesNotifications.Infra.Data.Context;
using FiapCloudGamesNotifications.Infra.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddElasticConfiguration();
builder.AddMassTransitConfiguration();

var serverVersion = new MySqlServerVersion(new Version(8, 0));
builder.Services.AddDbContext<ContextDb>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("MySQL"), serverVersion);
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Key"])),
        RoleClaimType = ClaimTypes.Role
    };
});
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUserNotificationProfileService, UserNotificationProfileService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SameUserOrAdmin", policy =>
        policy.Requirements.Add(new SameUserRequirement()));
});
builder.Services.AddScoped<IAuthorizationHandler, SameUserHandler>();

var app = builder.Build();

if (args.Contains("migrate"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ContextDb>();
    db.Database.Migrate();
    return;
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseHsts();

app.MapNotificationEndpoints();

app.Run();
