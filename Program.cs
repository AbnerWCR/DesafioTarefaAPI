using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TarefaAPI.Data;
using TarefaAPI.Enums;
using TarefaAPI.Interfaces;
using TarefaAPI.ViewModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => 
{
    options.UseSqlite(
        connectionString: builder.Configuration.GetConnectionString("default"))
        .EnableSensitiveDataLogging()
        .UseLoggerFactory(LoggerFactory.Create(build => build.AddConsole()));
});

#region "Injeção de dependências"

builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();

#endregion

builder.Services.AddEndpointsApiExplorer();

#region "Swagger"

builder.Services.AddSwaggerGen(c =>
{
    #region  "description api"

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User API",
        Version = "v1",
        Description = "API feita para desafio da plataforma DIO.me",
        Contact = new OpenApiContact
        {
            Name = "Abner Wallace",
            Email = "abnerwcrodrigues@gmail.com"
        },
    });

    #endregion
});

#endregion

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

#region "Rotas"

app.MapGet("/v1/Tarefa", async (
    ITarefaRepository repository) =>
{
    try
    {
        var tarefasRepo = await repository.GetAllAsync();
        var tarefaVm = new TarefaViewModel();

        return Results.Ok(tarefaVm.MapFromList(tarefasRepo));
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.StatusCode(500);
    }

}).AllowAnonymous()
.WithName("Obter todas as tarefas")
.WithTags("Tarefas")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status500InternalServerError);

app.MapGet("/v1/Tarefa/{id}", async (
    ITarefaRepository repository,
    int id) =>
{
    try
    {
        if (id <= 0)
            return Results.BadRequest("Id inválido");

        var tarefaRepo = await repository.GetByIdAsync(id);

        if (tarefaRepo == null)
            return Results.NotFound(0);

        var tarefaVm = new TarefaViewModel();
        return Results.Ok(tarefaVm.MapFrom(tarefaRepo));
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.StatusCode(500);
    }

}).AllowAnonymous()
.WithName("Obter tarefas por ID")
.WithTags("Tarefas")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status500InternalServerError);

app.MapGet("/v1/Tarefa/{titulo}/PorTitulo", async (
    ITarefaRepository repository,
    string titulo) =>
{
    try
    {
        if (string.IsNullOrEmpty(titulo))
            return Results.BadRequest("TITULO inválido");

        var tarefasRepo = await repository.GetByTitleAsync(titulo);

        if (!tarefasRepo.Any())
            return Results.NotFound(0);

        var tarefaVm = new TarefaViewModel();
        return Results.Ok(tarefaVm.MapFromList(tarefasRepo));
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.StatusCode(500);
    }

}).AllowAnonymous()
.WithName("Obter tarefas por TÍTULO")
.WithTags("Tarefas")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status500InternalServerError);

app.MapGet("/v1/Tarefa/{data}/PorData", async (
    ITarefaRepository repository,
    DateTime data) =>
{
    try
    {
        var tarefasRepo = await repository.GetByDateAsync(data);

        if (!tarefasRepo.Any())
            return Results.NotFound(0);

        var tarefaVm = new TarefaViewModel();
        return Results.Ok(tarefaVm.MapFromList(tarefasRepo));
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.StatusCode(500);
    }

}).AllowAnonymous()
.WithName("Obter tarefas por DATA")
.WithTags("Tarefas")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status500InternalServerError);

app.MapGet("/v1/Tarefa/{status}/PorStatus", async (
    ITarefaRepository repository,
    Status status) =>
{
    try
    {
        var tarefasRepo = await repository.GetByStatusAsync(status);

        if (!tarefasRepo.Any())
            return Results.NotFound(0);

        var tarefaVm = new TarefaViewModel();
        return Results.Ok(tarefaVm.MapFromList(tarefasRepo));
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.StatusCode(500);
    }

}).AllowAnonymous()
.WithName("Obter tarefas por STATUS")
.WithTags("Tarefas")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status500InternalServerError);

app.MapPost("/v1/Tarefa", async (
    ITarefaRepository repository,
    CriarTarefaViewModel vm) => 
{
    try
    {
        var tarefa = vm.MapTo();

        if (!vm.IsValid)
            return Results.BadRequest(vm.Notifications);

        var novaTarefa = await repository.CreateAsync(tarefa);
        var uri = new Uri($"/v1/Tarefa/{novaTarefa.Id}", UriKind.RelativeOrAbsolute);

        var tarefaVm = new TarefaViewModel();
        return Results.Created(uri, tarefaVm.MapFrom(novaTarefa));
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.StatusCode(500);
    }
})
.AllowAnonymous()
.WithName("Criar Tarefa")
.WithTags("Tarefas")
.Produces(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError);

app.MapPut("v1/Tarefa{id}", async (
    ITarefaRepository repository,
    int id,
    AlterarTarefaViewModel vm) => 
{
    try
    {
        if (!vm.IsValid)
            return Results.BadRequest(vm.Notifications);

        var tarefaRepo = await repository.GetByIdAsync(id);

        if (tarefaRepo == null)
            return Results.NotFound(vm);

        var tarefa = tarefaRepo with 
        {
            Titulo = vm.Titulo,
            Descricao = vm.Descricao,
            Status = vm.Status
        };

        var tarefaAlterada = await repository.UpdateAsync(tarefa);

        var tarefaVm = new TarefaViewModel();
        return Results.Ok(tarefaVm.MapFrom(tarefaAlterada));
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.StatusCode(500);
    }

})
.AllowAnonymous()
.WithName("Alterar Tarefa por ID")
.WithTags("Tarefas")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status500InternalServerError);

app.MapDelete("v1/Tarefa", async (
    ITarefaRepository repository,
    int id) => 
{
    try
    {
        if (id <= 0)
            return Results.BadRequest("Id inválido");

        var tarefaRepo = await repository.GetByIdAsync(id);

        if (tarefaRepo == null)
            return Results.NotFound(0);

        await repository.DeleteAsync(id);
        return Results.Ok(tarefaRepo);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.StatusCode(500);
    }
})
.AllowAnonymous()
.WithName("Deletar Tarefa por ID")
.WithTags("Tarefas")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status500InternalServerError); ;

#endregion

app.Run();
