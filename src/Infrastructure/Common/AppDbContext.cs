using Domain.Common;
using Domain.TodoLists;
using Domain.Users;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public class AppDbContext(DbContextOptions options, IPublisher publisher)
    : DbContext(options)
{
    public DbSet<TodoList> TodoLists { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // todo move after saving changes and deal with eventual consistency
        await PublishDomainEventsAsync();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .SelectMany(x => x.Entity.PopDomainEvents())
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent);
        }
    }
}