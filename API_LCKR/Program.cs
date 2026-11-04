using API_LCKR.Converters;
using API_LCKR.DTOs;
using API_LCKR.Mappers;
using API_LCKR.Models;
using API_LCKR.Repository;
using API_LCKR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using QuestPDF.Infrastructure;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

// Add services to the container.
var dataSourceBuilder = new NpgsqlDataSourceBuilder(
    builder.Configuration.GetConnectionString("Connection"));

dataSourceBuilder.MapEnum<StatusCoach>("status_coaches");
dataSourceBuilder.MapEnum<StatusLesson>("status_lessons");
dataSourceBuilder.MapEnum<StatusPagamento>("status_pagamento");
dataSourceBuilder.MapEnum<StatusPeriodo>("status_periodo");

var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<LckrDatabaseContext>(options =>
    options.UseNpgsql(dataSource));

builder.Services.AddScoped<CoachService>();
builder.Services.AddScoped<CoachRepository>();
builder.Services.AddScoped<CoachMapper>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<HistoricoValorService>();
builder.Services.AddScoped<HistoricoValorRepository>();
builder.Services.AddScoped<HistoricoValorMapper>();
builder.Services.AddScoped<AulaService>();
builder.Services.AddScoped<AulaRepository>();
builder.Services.AddScoped<AulaMapper>();
builder.Services.AddScoped<FolhaService>();
builder.Services.AddScoped<FolhaRepository>();
builder.Services.AddScoped<PeriodoService>();
builder.Services.AddScoped<PeriodoRepository>();
builder.Services.AddScoped<PeriodoMapper>();
builder.Services.AddScoped<PagamentoService>();
builder.Services.AddScoped<PagamentoRepository>();
builder.Services.AddScoped<PagamentoMapper>();
builder.Services.AddScoped<ExportacaoService>();
builder.Services.AddScoped<EmailService>();


builder.Services.AddControllers(options =>
{
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((valor, campo) => $"O valor '{valor}' não é válido para {campo}.");
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(valor => $"O valor '{valor}' não é válido.");
    options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(valor => $"O valor '{valor}' não é válido.");
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        options.JsonSerializerOptions.Converters.Add(new HoraJsonConverter());
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var jsonInvalido = context.ModelState.Keys.Any(chave => chave.StartsWith("$"));

            var mensagem = jsonInvalido
                ? "Corpo da requisição inválido. Verifique os campos e os valores permitidos."
                : string.Join(" ", context.ModelState.Values
                    .SelectMany(valor => valor.Errors)
                    .Select(erro => erro.ErrorMessage));

            return new BadRequestObjectResult(new ErroDto(mensagem));
        };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
