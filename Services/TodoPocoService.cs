using Microsoft.EntityFrameworkCore;
using EFCore.AutomaticMigrations.EF.Sample;
using EFCore.AutomaticMigrations.Context.TodoNew;

namespace EFCore.AutomaticMigrations.Sample.Services;

public class TodoPocoService(TodoDbContext dbContext) : ITodoPocoService
{
    private readonly TodoDbContext _dbContext = dbContext;

    public async ValueTask<TodoPoco?> FindAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TodosPoco.FindAsync([id], cancellationToken: cancellationToken);
    }

    public async Task<List<TodoPoco>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.TodosPoco.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TodoPoco todo, CancellationToken cancellationToken = default)
    {
        await _dbContext.TodosPoco.AddAsync(todo, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TodoPoco todo, CancellationToken cancellationToken = default)
    {
        _dbContext.TodosPoco.Update(todo);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(TodoPoco todo, CancellationToken cancellationToken = default)
    {
        _dbContext.TodosPoco.Remove(todo);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<List<TodoPoco>> GetIncompleteTodoPocosAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.TodosPoco.Where(t => t.IsDone == false).ToListAsync(cancellationToken);
    }
}
