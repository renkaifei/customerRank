using CustomerRank.App;
using CustomerRank.HostedServices;
using Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<Leaderboard>();
builder.Services.AddSingleton<CustomerSet>();
builder.Services.AddSingleton<LeaderboardChannel>();
builder.Services.AddHostedService<LeaderboardHostedService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
