
# EF Core Automatic Migrations for PostgreSQL

![.NET 9.0](https://img.shields.io/badge/.NET-9.0-blue) ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-supported-green) ![EF Core 9.0](https://img.shields.io/badge/EF%20Core-9.0-purple)

**Enable automatic Entity Framework Core migrations for PostgreSQL databases without manual migration files.**

This package is specifically optimized for PostgreSQL with support for schemas, bytea storage, UUID extensions, and PostgreSQL-specific data types.

# About
Contains instructions to use [PostgreSQL.EFCore.AutomaticMigrations.Npgsql](https://www.nuget.org/packages/PostgreSQL.EFCore.AutomaticMigrations.Npgsql/) nuget package for Automatic Migrations with Entity Framework Core PostgreSQL Databases.

## Quick Start

To see this package in action, you can use the minimal api sample project provided in the repository. The sample project demonstrates how to set up and use PostgreSQL.EFCore.AutomaticMigrations.Npgsql in a real-world scenario. Follow these steps to get started:
1. **Clone the repository:**

    ```sh
    git clone https://github.com/ionutneagos/efcore-automaticmigrations-PostgreSQL.git
    ```

2. **Open the solution:**

    Open the solution file `Npgsql.EFCore.AutomaticMigrations.PostgreSQL.Sample.sln` in your preferred IDE.

3. **Update the connection string:**

    Modify the connection string in `appsettings.json` to point to your SQL database.

4. **Run the application:**

    Start the application to see `PostgreSQL.EFCore.AutomaticMigrations.Npgsql` in action.

The sample project includes examples of how to configure and use the `DbMigrationsOptions` object, apply migrations, reset the database schema, and view applied migrations. This should help you understand how to integrate EFCore.AutomaticMigrations into your own projects.

Relevant lines of code are:

```cs
// create builder app
var app = builder.Build();

// begin apply automatic migration database to latest version
await using AsyncServiceScope serviceScope = app.Services.CreateAsyncScope();

// ToDoDBContext - entity framework core database context class
await using TodoDbContext? dbContext = serviceScope.ServiceProvider.GetService<TodoDbContext>();

if (dbContext is not null)
{
    // If the database context was successfully resolved from the service provider, we apply migrations.
    // The DbMigrationsOptions object is used to configure automatic data loss prevention and offers other tools like viewing raw SQL scripts for migrations.
    // The database is created automatically if it does not exist, and if it exists, it will be updated to the latest model changes.
    // Pay attention if you are using a PaaS database, like Azure; it will be created automatically using the default SKU, which might affect your costs.

    await dbContext.MigrateToLatestVersionAsync(new DbMigrationsOptions { AutomaticMigrationDataLossAllowed = true });

    // At this stage, the database contains the latest changes.
    // The Todo entity was mapped via Fluent API.
    // The TodoPoco entity was mapped via data annotations.
}
```

## Usage

### Context methods

* Execute(TContext context,TMigrationsOptions options);
* ExecuteAsync(TContext context,TMigrationsOptions options,CancellationToken cancellationToken);
  
### Extensions methods

* MigrateToLatestVersion<TContext, TMigrationsOptions>(this TContext context, TMigrationsOptions options);
* MigrateToLatestVersionAsync<TContext, TMigrationsOptions>(this TContext context, TMigrationsOptions options);

### DbMigrationsOptions object allows to configure migration options:

```cs
 /// <summary>
 /// Gets or sets a value indicating whether automatic migrations that could result in data loss are allowed  (default: false).
 /// </summary>
 public bool AutomaticMigrationDataLossAllowed { get; set; } = false;

 /// <summary>
 /// Gets or sets a value indicating whether automatic migrations are enabled  (default: true). 
 /// </summary>
 public bool AutomaticMigrationsEnabled { get; set; } = true;

 /// <summary>
 /// Gets or sets a value indicating whether to reset the database schema by dropping all tables and recreating them (default: false).
 /// Useful in scenarios when the data is transient and can be dropped when the schema changes. For example during prototyping, in tests, or for local changes
 /// When ResetDatabaseSchema is true AutomaticMigrationsEnabled and AutomaticMigrationDataLossAllowed are set to true
 /// </summary>
 public bool ResetDatabaseSchema { get; set; } = false;

 /// <summary>
 /// Gets or sets a dictionary of key-value pairs for updating the model snapshot.
 /// This property allows for dynamic modification of the generated model snapshot code by performing string replacements during the migration process.
 /// </summary>
 public Dictionary<string, string> UpdateSnapshot { get; set; } = [];

 /// <summary>
 /// PostgreSQL schema tables (default: "public")
 /// </summary>
 public string? Schema { get; set; } = null;

 ```

#### Migration code

 ```cs
  // Get context
  var context = services.GetRequiredService<YourContext>();

  // Apply migrations
  context.MigrateToLatestVersion();

 // Reset database schema sync
  context.MigrateToLatestVersion(new DbMigrationsOptions { ResetDatabaseSchema = true });

  // Apply migrations async
  context.MigrateToLatestVersionAsync();

  // Reset database schema async
  await context.MigrateToLatestVersionAsync(new DbMigrationsOptions { ResetDatabaseSchema = true });
  ```

### Helpers methods

* ListAppliedMigrationsAsync(context, options, cancellationToken) - list applied migration
* ListMigrationOperationsAsRawSqlAsync(context, options, cancellationToken) - view the raw SQL  being executed, which can log or save if needed.

 ```cs
 /// <summary>
/// List applied migration
/// </summary>
/// <returns>List of applied migration</returns>
 List<MigrationRaw> ListAppliedMigrations();
 async Task<List<MigrationRaw>> ListAppliedMigrationsAsync(CancellationToken cancellationToken = default);


 /// <summary>
 /// Use this method to list migration operations which will be applied as raw sql
 /// </summary>
 /// <returns>List of sql scripts. Empty list if no pending migrations, or database is not connected/created</returns>
 List<MigrationOperationRaw> ListMigrationOperationsAsRawSql();
 async Task<List<MigrationOperationRaw>> ListMigrationOperationsAsRawSqlAsync(CancellationToken cancellationToken = default);
 ```

```cs
// begin apply automatic migration database to latest version
await using AsyncServiceScope serviceScope = app.Services.CreateAsyncScope();

await using TodoDbContext? dbContext = serviceScope.ServiceProvider.GetService<TodoDbContext>();

if (dbContext is not null)
{
    // List migration operations as raw SQL commands
    var sqlMigrationOperations = await dbContext.ListMigrationOperationsAsRawSqlAsync();

    foreach (var sqlMigrationOperation in sqlMigrationOperations)
    {
        // log sql commands
        Console.WriteLine(sqlMigrationOperation.SqlCommand);
    }

    // Apply migrations
    await dbContext.MigrateToLatestVersionAsync();

    // List applied migrations
    List<MigrationRaw> appliedMigrations = await dbContext.ListAppliedMigrationsAsync();
    foreach (MigrationRaw migration in appliedMigrations)
    {
        Console.WriteLine(migration.MigrationId);
    }
}
```

### PostgreSQL Data Types Support

* **UUID** with `uuid-ossp` extension
* **JSON/JSONB** columns
* **Arrays** (`string[]`, `int[]`, etc.)
* **Timestamp**
* **Bytea** for binary data
* **Custom schemas**



## 📦 Dependencies & Compatibility

* **.NET 9.0+**
* **Microsoft.EntityFrameworkCore 9.0+**
* **Npgsql.EntityFrameworkCore.PostgreSQL 9.0+**
* **PostgreSQL 12+** (recommended PostgreSQL 14+)

## Important Notes

* **Backup First**: Always backup production databases before migrations
* **Test Thoroughly**: Test all migrations in staging environments
* **Monitor Performance**: Large schema changes may require maintenance windows
* **Permission Requirements**: Database user needs DDL permissions (CREATE, ALTER, DROP)

## License & Support

* **License**: MIT License
* **Issues**: [GitHub Issues](https://github.com/ionutneagos/efcore-automaticmigrations-PostgreSQL/issues)
* **Documentation**: [GitHub Repository](https://github.com/ionutneagos/efcore-automaticmigrations-PostgreSQL)
