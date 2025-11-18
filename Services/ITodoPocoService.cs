using EFCore.AutomaticMigrations.EF.Sample;
namespace EFCore.AutomaticMigrations.Sample.Services;

public interface ITodoPocoService
{
    Task<List<TodoPoco>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<TodoPoco>> GetIncompleteTodoPocosAsync(CancellationToken cancellationToken = default);

    ValueTask<TodoPoco?> FindAsync(int id, CancellationToken cancellationToken = default);

    Task AddAsync(TodoPoco todo, CancellationToken cancellationToken = default);

    Task UpdateAsync(TodoPoco todo, CancellationToken cancellationToken = default);

    Task RemoveAsync(TodoPoco todo, CancellationToken cancellationToken = default);
}
