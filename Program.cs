using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EventContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConection")));

builder.Services.AddScoped<ITipoUsuario, TipoUsuarioRepository>();
builder.Services.AddScoped<ITipoEvento, TipoEventoRepository>();

//Registra o serviço de controller, mapeia automaticamente o serviço da pasta/Controllers
builder.Services.AddControllers();

var app = builder.Build();

//Mapeia as rotas definidas nos controllers, com os atributos [Route]: api/controller 
app.MapControllers();

app.Run();
