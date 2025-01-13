using System.Net;
using DataAccess.Context;
using DataAccess.Factories;
using Interface.Factories;
using Logic.Factories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MotoForce_api.Hubs;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 8080);
});

// Add services to the container.
builder.Services.AddHttpClient();

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ILogicFactoryBuilder, LogicFactoryBuilder>();
builder.Services.AddScoped<IDalFactory, DalFactory>();
builder.Services.AddScoped<IHandlerFactory, HandlerFactory>();
builder.Services.AddScoped<NotificationHub>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// Configure CORS to allow all origins, methods, and headers
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use the CORS policy that allows all origins
app.UseCors(config =>
    config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Map signalR hub
app.UseWebSockets();
app.MapHub<NotificationHub>("/notificationHub");
app.Logger.LogInformation("WebSocket is running on ws://{0}:{1}/notificationHub", "localhost", 8080);


app.Run();