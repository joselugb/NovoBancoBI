using Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BancoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);
builder.Services.AddScoped<IBancoDbContext>(sp =>
    sp.GetRequiredService<BancoDbContext>());

builder.Services.AddScoped<Application.Servicios.TransaccionServicio>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();