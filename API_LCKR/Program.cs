using API_LCKR.Converters;
using API_LCKR.DTOs;
using API_LCKR.Mappers;
using API_LCKR.Models;
using API_LCKR.Repository;
using API_LCKR.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using QuestPDF.Infrastructure;
using System.Text;
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

var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrWhiteSpace(jwtKey) || Encoding.UTF8.GetByteCount(jwtKey) < 32)
{
    throw new InvalidOperationException("Configure a chave Jwt:Key com pelo menos 32 caracteres.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});


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
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Cole aqui o token devolvido pelo login"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://gestao.lckrclub.com")
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
