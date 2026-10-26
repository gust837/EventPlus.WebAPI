# ==================== ETAPA 1: COMPILAR O CÓDIGO C# ====================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copia primeiro só o arquivo .csproj e baixa os pacotes NuGet (BCrypt, EF Core, Cloudinary...)
# Fazemos isso separado porque o Docker guarda os pacotes em cache e os próximos builds ficam instantâneos!
COPY ["EventPlus.WebAPI.csproj", "./"]
RUN dotnet restore "EventPlus.WebAPI.csproj"

# 2. Agora copia todo o resto do código C# e gera a versão de produção na pasta /app/publish
COPY . .
RUN dotnet publish "EventPlus.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ==================== ETAPA 2: RODAR A API ====================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Expõe a porta 8080 (porta padrão do .NET dentro do Docker)
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Copia apenas os arquivos compilados lá da Etapa 1
COPY --from=build /app/publish .

# Comando que liga a API quando o container iniciar
ENTRYPOINT ["dotnet", "EventPlus.WebAPI.dll"]