using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Cold.FluentMapping
{
    public class CategoryMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Nome da tabela
            builder.ToTable("Categories");

            // Chave primária
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("Id")
                .HasColumnType("uuid")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(c => c.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("now()")
                .IsRequired();

            builder.Property(c => c.DeletedDate)
                .HasColumnName("DeletedDate")
                .HasColumnType("timestamp")
                .IsRequired(false);

            // Mapeamento do Value Object UniqueName para Name
            builder.OwnsOne(c => c.Name, name =>
            {
                name.Property(n => n.Name)
                    .HasColumnName("Name")
                    .HasColumnType("varchar")
                    .HasMaxLength(100)
                    .IsRequired();
            });

            builder.Property(c => c.Active)
                .HasColumnName("Active")
                .HasColumnType("boolean")
                .IsRequired();

            builder.HasMany(c => c.Apps)
                .WithOne(a => a.Category)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
