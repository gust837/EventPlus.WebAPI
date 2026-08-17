using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EventContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConection")));

//Corta o ciclo Usuario --> TipoUsuario --> 
//colocando um null no ponto onde a referencia se repete
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddScoped<ITipoUsuario, TipoUsuarioRepository>();
builder.Services.AddScoped<ITipoEvento, TipoEventoRepository>();
builder.Services.AddScoped<IUsuario, UsuarioRepository>();
builder.Services.AddScoped<IInstituicao, InstituicaoRepository>();
builder.Services.AddScoped<IEvento, EventoRepository>();

//Registra o serviço de controller, mapeia automaticamente o serviço da pasta/Controllers
builder.Services.AddControllers();

var app = builder.Build();

//Mapeia as rotas definidas nos controllers, com os atributos [Route]: api/controller 
app.MapControllers();

app.Run();
