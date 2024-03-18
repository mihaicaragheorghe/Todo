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

        builder.Property(u => u.PasswordHash)
            .IsRequired();
        
        builder.Property(u => u.Roles)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.HasIndex(u => u.Email);
    }
}
