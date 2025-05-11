using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Cold.FluentMapping
{
    public class AppMapping : IEntityTypeConfiguration<App>
    {
        public void Configure(EntityTypeBuilder<App> builder)
        {
            builder.ToTable("Apps");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("Id")
                .HasColumnType("uuid")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(a => a.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(a => a.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(a => a.DeletedDate)
                .HasColumnName("DeletedDate")
                .HasColumnType("timestamp")
                .IsRequired(false);

            builder.Property(a => a.Environment)
                .HasColumnName("Environment")
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(a => a.Active)
                .HasColumnName("Active")
                .HasColumnType("boolean")
                .IsRequired();

            builder.OwnsOne(a => a.Name, name =>
            {
                name.Property(n => n.Name)
                    .HasColumnName("Name")
                    .HasColumnType("varchar")
                    .HasMaxLength(100)
                    .IsRequired();
            });

            builder.HasOne(a => a.Category)
                .WithMany(c => c.Apps)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Logs)
                .WithOne(l => l.App)
                .HasForeignKey(l => l.AppId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
