using Application.Servicios;
using Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;
// using Microsoft.Identity.Client.AppConfig;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<BancoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<TransaccionServicio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapControllers();
}

app.UseHttpsRedirection();

