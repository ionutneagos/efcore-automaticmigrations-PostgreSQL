using EFCore.AutomaticMigrations.EF.Sample;
using Microsoft.EntityFrameworkCore;

namespace EFCore.AutomaticMigrations.Context.TodoNew;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos => base.Set<Todo>();
    public DbSet<TodoPoco> TodosPoco => Set<TodoPoco>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasPostgresExtension("uuid-ossp"); // if using uuid generation
        //OBS!: you can filter types within the assembly based on context name, usefull on multitenant solutions
        builder.ApplyConfigurationsFromAssembly(typeof(TodoDbContext).Assembly);
    }

}
