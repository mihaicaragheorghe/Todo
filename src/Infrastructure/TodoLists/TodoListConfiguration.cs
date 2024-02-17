using Domain.TodoLists;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.TodoLists;

public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.HasKey(tl => tl.Id);

        builder.Property(tl => tl.Id)
            .ValueGeneratedNever();

        builder.Property(tl => tl.Name)
            .IsRequired()
            .HasMaxLength(TodoListConstants.NameMaxLength);

        builder.HasMany(tl => tl.Todos)
            .WithOne()
            .HasForeignKey(t => t.ListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}