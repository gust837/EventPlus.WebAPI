using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Repositories;
using EventPlus.WebAPI.Service;
using EventPlus.WebAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Adicionando a extensão do Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Insira um token válido para ter acesso aos endpoints da API"
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

builder.Services.AddDbContext<EventContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Corta o ciclo Usuario --> TipoUsuario colocando null no ponto onde a referência se repete
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddScoped<ITipoUsuario, TipoUsuarioRepository>();
builder.Services.AddScoped<ITipoEvento, TipoEventoRepository>();
builder.Services.AddScoped<IUsuario, UsuarioRepository>();
builder.Services.AddScoped<IInstituicao, InstituicaoRepository>();
builder.Services.AddScoped<IEvento, EventoRepository>();
builder.Services.AddScoped<IComentario, ComentarioRepository>();
builder.Services.AddScoped<IPresenca, PresencaRepository>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

// Lê as URLs permitidas no CORS do appsettings.json / variáveis de ambiente (mantendo localhost por padrão)
var origensConfiguradas = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];
var frontendUrlExtra = builder.Configuration["FRONTEND_URL"];
var origensPermitidas = origensConfiguradas
    .Concat(["http://localhost:5173", "http://localhost:3000"])
    .Concat(string.IsNullOrWhiteSpace(frontendUrlExtra) ? [] : [frontendUrlExtra])
    .Distinct()
    .ToArray();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirReact", policy =>
    {
        policy.WithOrigins(origensPermitidas)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Registra o HttpClient tipado para o serviço de moderação de texto (Sightengine)
builder.Services.AddHttpClient<IModeracaoTextoService, ModeracaoTextoService>(client =>
{
    client.BaseAddress = new Uri("https://api.sightengine.com/1.0/");
});

// Autenticação JWT: configura como a API vai validar os tokens recebidos nas requisições
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Valida quem emitiu o token
        ValidateIssuer = true,
        ValidIssuer = "EventPlus.WebAPI",

        // Valida para quem o token foi emitido
        ValidateAudience = true,
        ValidAudience = "EventPlus.WebAPI",

        // Valida se o token ainda está dentro do prazo
        ValidateLifetime = true,

        // Define a tolerância de clock entre servidores
        ClockSkew = TimeSpan.FromMinutes(5),

        // Chave secreta utilizada para validar a assinatura do token
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// Configuração do Cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

// Registra o serviço de autorização
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirReact");

// Redireciona Http para Https automaticamente
app.UseHttpsRedirection();

// Ativa a autenticação
app.UseAuthentication();

// Ativa a autorização
app.UseAuthorization();

// Mapeia as rotas definidas nos controllers com [Route("api/[controller]")]
app.MapControllers();

app.Run();
