using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasColumnName("Id")
                .HasColumnType("UUID")
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(u => u.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("DateTime")
                .HasDefaultValueSql("now()")
                .IsRequired();
            builder.Property(u => u.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("DateTime")
                .HasDefaultValueSql("now()")
                .IsRequired();
            builder.Property(u => u.DeletedDate)
                .HasColumnName("DeletedDate")
                .HasColumnType("DateTime")
                .IsRequired(false);
            builder.OwnsOne(u => u.FullName, fullName =>
            {
                fullName.Property(f => f.FirstName)
                    .HasColumnName("FirstName")
                    .HasColumnType("String")
                    .HasMaxLength(100)
                    .IsRequired();
                fullName.Property(f => f.LastName)
                    .HasColumnName("LastName")
                    .HasColumnType("String")
                    .HasMaxLength(100)
                    .IsRequired();
            });
            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Address)
                    .HasColumnName("Email")
                    .HasColumnType("String")
                    .HasMaxLength(50)
                    .IsRequired();
            });
            builder.OwnsOne(u => u.Address, address =>
            {
                address.Property(a => a.Road)
                    .HasColumnName("Road")
                    .HasColumnType("String")
                    .HasMaxLength(100)
                    .IsRequired(false);
                address.Property(a => a.Number)
                    .HasColumnName("Number")
                    .HasColumnType("Int64")
                    .IsRequired(false);
                address.Property(a => a.NeighBordHood)
                    .HasColumnName("NeighborHood")
                    .HasColumnType("String")
                    .HasMaxLength(100)
                    .IsRequired(false);
                address.Property(a => a.Complement)
                    .HasColumnName("Complement")
                    .HasColumnType("String")
                    .HasMaxLength(100)
                    .IsRequired(false);
            });
            builder.Property(u => u.TokenActivate)
                .HasColumnName("TokenActivate")
                .HasColumnType("UUID")
                .IsRequired(false);
            builder.Property(u => u.Active)
                .HasColumnName("Active")
                .HasColumnType("Bool")
                .IsRequired();
            builder.OwnsOne(u => u.Password, password =>
            {
                password.Property(p => p.Hash)
                    .HasColumnName("Hash")
                    .HasColumnType("String")
                    .IsRequired(false);
                password.Property(p => p.Salt)
                    .HasColumnName("Salt")
                    .HasColumnType("String")
                    .IsRequired(false);
                password.Ignore(p => p.Content);
            });
        }
    }
}
