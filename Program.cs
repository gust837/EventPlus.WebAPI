using EventPlus.WebAPI.Contexts;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EventContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConection")));

builder.Services.AddScoped<ITipoUsuario, TipoUsuarioRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
