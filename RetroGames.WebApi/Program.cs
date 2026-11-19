using RetroGames.WebApi.Configuration;

// Carrega o .env (procura a partir da pasta atual, subindo até achar).
// Precisa vir antes do builder, para as variáveis entrarem na configuração.
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// ConnectionStrings__Default do .env vira ConnectionStrings:Default aqui.
var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string 'Default' não encontrada. Confira a linha " +
        "ConnectionStrings__Default no arquivo .env, na raiz do repositório.");
}

builder.Services.AddRepositorios(connectionString);
builder.Services.AddServicosDeNegocio();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();   // descrição da API em /openapi/v1.json
    app.UseSwaggerUI(opcoes => opcoes.SwaggerEndpoint("/openapi/v1.json", "RetroGames API"));
}

app.MapControllers();

app.Run();
