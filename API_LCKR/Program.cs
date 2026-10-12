using API_LCKR.Mappers;
using API_LCKR.Models;
using API_LCKR.Repository;
using API_LCKR.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dataSourceBuilder = new NpgsqlDataSourceBuilder(
    builder.Configuration.GetConnectionString("Connection"));

dataSourceBuilder.MapEnum<StatusCoach>("status_coaches");
dataSourceBuilder.MapEnum<StatusLesson>("status_lessons");
dataSourceBuilder.MapEnum<StatusPagamento>("status_pagamento");

var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<LckrDatabaseContext>(options =>
    options.UseNpgsql(dataSource));

builder.Services.AddScoped<CoachService>();
builder.Services.AddScoped<CoachRepository>();
builder.Services.AddScoped<CoachMapper>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
