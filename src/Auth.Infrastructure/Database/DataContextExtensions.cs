using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Database;

public static class DataContextExtensions
{
    public static async Task ExecuteWithTransactionAsync(
        this DataContext dataContext,
        Func<Task> action,
        IsolationLevel isolationLevel = IsolationLevel.Snapshot,
        CancellationToken cancellationToken = default)
    {
        var executionStrategy = dataContext.Database.CreateExecutionStrategy();

        await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dataContext.Database.BeginTransactionAsync(
                isolationLevel,
                cancellationToken);

            await action();
            await transaction.CommitAsync(cancellationToken);
        });
    }

    public static async Task<TResponse> ExecuteWithTransactionAsync<TResponse>(
        this DataContext dataContext,
        Func<Task<TResponse>> func,
        IsolationLevel isolationLevel = IsolationLevel.Snapshot,
        CancellationToken cancellationToken = default)
    {
        var executionStrategy = dataContext.Database.CreateExecutionStrategy();

        var response = await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dataContext.Database.BeginTransactionAsync(
                isolationLevel,
                cancellationToken);

            var response = await func();
            await transaction.CommitAsync(cancellationToken);
            return response;
        });

        return response;
    }
}