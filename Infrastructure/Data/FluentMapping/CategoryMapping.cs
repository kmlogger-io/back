using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping
{
    public class CategoryMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasColumnName("Id")
                .HasColumnType("UUID")
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(c => c.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("DateTime")
                .HasDefaultValueSql("now()")
                .IsRequired();
            builder.Property(c => c.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("DateTime")
                .HasDefaultValueSql("now()")
                .IsRequired();
            builder.Property(c => c.DeletedDate)
                .HasColumnName("DeletedDate")
                .HasColumnType("DateTime")
                .IsRequired(false);
            builder.OwnsOne(c => c.Name, name =>
            {
                name.Property(n => n.Name)
                    .HasColumnName("Name")
                    .HasColumnType("String")
                    .HasMaxLength(100)
                    .IsRequired();
            });
            builder.Property(c => c.Active)
                .HasColumnName("Active")
                .HasColumnType("Bool")
                .IsRequired(false);
        }
    }
}
