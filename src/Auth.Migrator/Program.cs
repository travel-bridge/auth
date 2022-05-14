using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__AuthDatabase")
    ?? throw new InvalidOperationException("Connection string is not configured.");

var executingAssembly = Assembly.GetExecutingAssembly();

var serviceProvider = new ServiceCollection()
    .AddFluentMigratorCore()
    .ConfigureRunner(builder =>
    {
        builder
            .AddPostgres()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(executingAssembly);
    })
    .AddLogging(builder => builder.AddFluentMigratorConsole())
    .Configure<FluentMigratorLoggerOptions>(options =>
    {
        options.ShowSql = true;
        options.ShowElapsedTime = true;
    })
    .BuildServiceProvider();

var migrator = serviceProvider.GetRequiredService<IMigrationRunner>();

migrator.MigrateUp();