using FluentValidation;
using RetroGames.Services;
using RetroGames.WebApi.Configuration;
using RetroGames.WebApi.Filters;
using RetroGames.WebApi.Middlewares;

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

// AdminInicial__Email e AdminInicial__Senha do .env.
var emailAdministrador = builder.Configuration["AdminInicial:Email"];
var senhaAdministrador = builder.Configuration["AdminInicial:Senha"];
if (string.IsNullOrWhiteSpace(emailAdministrador) || string.IsNullOrWhiteSpace(senhaAdministrador))
{
    throw new InvalidOperationException(
        "Administrador inicial não configurado. Confira as linhas " +
        "AdminInicial__Email e AdminInicial__Senha no arquivo .env.");
}

builder.Services.AddRepositorios(connectionString);
builder.Services.AddServicosDeNegocio();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Registra todos os validators deste projeto.
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services
    .AddControllers(opcoes => opcoes.Filters.Add<FluentValidationFilter>())
    .ConfigureApiBehaviorOptions(opcoes =>
    {
        // JSON malformado ou tipo errado: o [ApiController] recusa antes da action.
        // Aqui trocamos a resposta padrão dele pelo nosso envelope, com código 3.
        opcoes.InvalidModelStateResponseFactory = contexto =>
            FluentValidationFilter.RespostaInvalida(contexto.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    e => e.Key,
                    e => e.Value!.Errors.Select(erro => erro.ErrorMessage).ToArray()));
    });

builder.Services.AddOpenApi();

var app = builder.Build();

// Cria o administrador inicial, se ainda não houver nenhum.
// Services "scoped" só existem dentro de um escopo, por isso o CreateScope.
using (var escopo = app.Services.CreateScope())
{
    var autenticacaoService = escopo.ServiceProvider.GetRequiredService<IAutenticacaoService>();
    await autenticacaoService.GarantirAdministradorInicialAsync(emailAdministrador, senhaAdministrador);
}

// Primeira etapa do pipeline: captura exceções de tudo o que vem depois.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();   // descrição da API em /openapi/v1.json
    app.UseSwaggerUI(opcoes => opcoes.SwaggerEndpoint("/openapi/v1.json", "RetroGames API"));
}

app.MapControllers();

app.Run();
