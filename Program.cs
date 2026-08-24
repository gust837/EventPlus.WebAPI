using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Repositories;
using EventPlus.WebAPI.Service;
using EventPlus.WebAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EventContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Corta o ciclo Usuario --> TipoUsuario --> 
//colocando um null no ponto onde a referencia se repete
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddScoped<ITipoUsuario, TipoUsuarioRepository>();
builder.Services.AddScoped<ITipoEvento, TipoEventoRepository>();
builder.Services.AddScoped<IUsuario, UsuarioRepository>();
builder.Services.AddScoped<IInstituicao, InstituicaoRepository>();
builder.Services.AddScoped<IEvento, EventoRepository>();
builder.Services.AddScoped<IComentario, ComentarioRepository>();
builder.Services.AddScoped<IPresenca, PresencaRepository>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

//Autenticaçao JWT configura como a API vai validar os tokens recebidos nas requisiçoes
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        //valida quem emitiu o token
        ValidateIssuer = true,
        ValidIssuer = "EventPlus.WebAPI",

        //valida para quem o token foi emitido
        ValidateAudience = true,
        ValidAudience = "EventPlus.WebAPI",

        //valida se o token ainda esta dentro do prazo
        ValidateLifetime = true,


        //define a tolerancia de clock entre servidores
        ClockSkew = TimeSpan.FromMinutes(5),

        //chave secreta utilizada para validar a assinatura do token
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("Jwt:Key"))
    };
});

//Configuração do Cloudnary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

//Reistra o serviço de autorização
builder.Services.AddAuthorization();

//Registra o serviço de controller, mapeia automaticamente o serviço da pasta/Controllers
builder.Services.AddControllers();

var app = builder.Build();

//Redireciona Http para Https automaticamente
app.UseHttpsRedirection();

//Ativa a autenticação
app.UseAuthentication();

//ativa a autorização
app.UseAuthorization();

//Mapeia as rotas definidas nos controllers, com os atributos [Route]: api/controller 
app.MapControllers();

app.Run();
