using Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BancoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);


builder.Services.AddScoped<IBancoDbContext>(sp =>
    sp.GetRequiredService<BancoDbContext>());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();