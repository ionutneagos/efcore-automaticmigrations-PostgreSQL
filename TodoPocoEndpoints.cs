using Microsoft.AspNetCore.Http.HttpResults;
using EFCore.AutomaticMigrations.Sample.Data.Dto;
using EFCore.AutomaticMigrations.Sample.Services;
using EFCore.AutomaticMigrations.EF.Sample;
namespace EFCore.AutomaticMigrations.Sample;

public static class TodoPocoEndpoints
{
    public static RouteGroupBuilder MapTodoPocosApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllTodoPocos);
        group.MapGet("/incompleted", GetAllIncompletedTodoPocos);
        group.MapGet("/{id}", GetTodoPoco);

        group.MapPost("/", CreateTodoPoco)
            .AddEndpointFilter(async (efiContext, next) =>
            {
                var param = efiContext.GetArgument<TodoDto>(0);

                var validationErrors = Utilities.IsValid(param);

                if (validationErrors.Count != 0)
                {
                    return Results.ValidationProblem(validationErrors);
                }

                return await next(efiContext);
            });

        group.MapPut("/{id}", UpdateTodoPoco);
        group.MapDelete("/{id}", DeleteTodoPoco);

        return group;
    }

    // get all todos
    public static async Task<Ok<List<TodoPoco>>> GetAllTodoPocos(ITodoPocoService todoService, CancellationToken cancellationToken = default)
    {
        var todos = await todoService.GetAllAsync(cancellationToken);
        return TypedResults.Ok(todos);
    }

    public static async Task<Ok<List<TodoPoco>>> GetAllIncompletedTodoPocos(ITodoPocoService todoService, CancellationToken cancellationToken = default)
    {
        var todos = await todoService.GetIncompleteTodoPocosAsync(cancellationToken);
        return TypedResults.Ok(todos);
    }

    // get todo by id
    public static async Task<Results<Ok<TodoPoco>, NotFound>> GetTodoPoco(int id, ITodoPocoService todoService, CancellationToken cancellationToken = default)
    {
        var todo = await todoService.FindAsync(id, cancellationToken);

        if (todo != null)
        {
            return TypedResults.Ok(todo);
        }

        return TypedResults.NotFound();
    }

    // create todo
    public static async Task<Created<TodoPoco>> CreateTodoPoco(TodoDto todo, ITodoPocoService todoService, CancellationToken cancellationToken = default)
    {
        var newTodoPoco = new TodoPoco
        {
            Title = todo.Title,
            Description = todo.Description,
            IsDone = todo.IsDone
        };

        await todoService.AddAsync(newTodoPoco, cancellationToken);

        return TypedResults.Created($"/todos/v1/{newTodoPoco.Id}", newTodoPoco);
    }

    // update todo
    public static async Task<Results<Created<TodoPoco>, NotFound>> UpdateTodoPoco(TodoPoco todo, ITodoPocoService todoService, CancellationToken cancellationToken = default)
    {
        var existingTodoPoco = await todoService.FindAsync(todo.Id, cancellationToken);

        if (existingTodoPoco != null)
        {
            existingTodoPoco.Title = todo.Title;
            existingTodoPoco.Description = todo.Description;
            existingTodoPoco.IsDone = todo.IsDone;

            await todoService.UpdateAsync(existingTodoPoco, cancellationToken);

            return TypedResults.Created($"/todos/v1/{existingTodoPoco.Id}", existingTodoPoco);
        }

        return TypedResults.NotFound();
    }

    // delete todo
    public static async Task<Results<NoContent, NotFound>> DeleteTodoPoco(int id, ITodoPocoService todoService, CancellationToken cancellationToken = default)
    {
        var todo = await todoService.FindAsync(id, cancellationToken);

        if (todo != null)
        {
            await todoService.RemoveAsync(todo, cancellationToken);
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}