using Domain.Common;
using Domain.TodoLists;

using Infrastructure.Common;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TodoLists;

public class TodoListRepository(AppDbContext dbContext) : ITodoListRepository
{
    public async Task AddAsync(TodoList todoList, CancellationToken cancellationToken = default)
    {
        await dbContext.TodoLists.AddAsync(todoList, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TodoList todoList, CancellationToken cancellationToken = default)
    {
        dbContext.TodoLists.Remove(todoList);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<TodoList>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.TodoLists
            .AsNoTracking()
            .Where(tl => tl.UserId == userId)
            .Include(tl => tl.Todos)
            .ToListAsync(cancellationToken);
    }

    public async Task<TodoList?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.TodoLists
            .Include(tl => tl.Todos)
            .FirstOrDefaultAsync(tl => tl.Id == id, cancellationToken);
    }

    public Task UpdateAsync(TodoList todoList, CancellationToken cancellationToken = default)
    {
        dbContext.TodoLists.Update(todoList);
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ReorderItems(List<ItemOrder> itemOrders, CancellationToken cancellationToken = default)
    {
        var ids = itemOrders.ConvertAll(io => io.ItemId);
        var lists = await dbContext.TodoLists
            .Where(list => ids.Contains(list.Id))
            .ToListAsync(cancellationToken);

        foreach (var list in lists)
        {
            var itemOrder = itemOrders.Single(io => io.ItemId == list.Id);

            list.SetOrder(itemOrder.Order);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}