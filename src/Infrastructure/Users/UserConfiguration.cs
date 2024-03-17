using Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(UserConstants.NameMaxLength);

        builder.Property(u => u.ProfilePictureUrl)
            .HasMaxLength(256);

        // builder.HasMany(u => u.Roles)
        //     .WithOne()
        //     .HasForeignKey("UserId")
        //     .IsRequired()
        //     .OnDelete(DeleteBehavior.Cascade);
    }
}
