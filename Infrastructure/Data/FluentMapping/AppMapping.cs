using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping
{
    public class AppMapping : IEntityTypeConfiguration<App>
    {
        public void Configure(EntityTypeBuilder<App> builder)
        {
            builder.ToTable("Apps");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("Id")
                .HasColumnType("UUID")
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(a => a.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("DateTime")
                .HasDefaultValueSql("now()")
                .IsRequired();
            builder.Property(a => a.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("DateTime")
                .HasDefaultValueSql("now()")
                .IsRequired();
            builder.Property(a => a.DeletedDate)
                .HasColumnName("DeletedDate")
                .HasColumnType("DateTime")
                .IsRequired(false);
            builder.Property(a => a.Environment)
                .HasColumnName("Environment")
                .HasConversion<string>()
                .HasColumnType("String")
                .IsRequired(false);
            builder.Property(a => a.Active)
                .HasColumnName("Active")
                .HasColumnType("Bool")
                .IsRequired(false);
            builder.OwnsOne(a => a.Name, name =>
            {
                name.Property(n => n.Name)
                    .HasColumnName("Name")
                    .HasColumnType("String")
                    .HasMaxLength(100)
                    .IsRequired();
            });
            builder.Property(a => a.CategoryId)
                .HasColumnName("CategoryId")
                .HasColumnType("UUID")
                .IsRequired();
        }
    }
}
