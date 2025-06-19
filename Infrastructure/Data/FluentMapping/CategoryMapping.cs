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
                .HasColumnType("uuid")
                .ValueGeneratedOnAdd()
                .IsRequired();
            
            builder.Property(c => c.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();
            
            builder.Property(c => c.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();
            
            builder.Property(c => c.DeletedDate)
                .HasColumnName("DeletedDate")
                .HasColumnType("timestamptz")
                .IsRequired(false);
            
            builder.OwnsOne(c => c.Name, name =>
            {
                name.Property(n => n.Name)  
                    .HasColumnName("CategoryName")  
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100)
                    .IsRequired();
                
                name.HasIndex(n => n.Name)
                    .IsUnique()
                    .HasDatabaseName("IX_Categories_Name_Unique");
            });
            
            builder.Property(c => c.Active)
                .HasColumnName("Active")
                .HasColumnType("boolean")
                .HasDefaultValue(true)
                .IsRequired(false);
            
            builder.HasIndex(c => c.CreatedDate)
                .HasDatabaseName("IX_Categories_CreatedDate");
            
            builder.HasIndex(c => c.Active)
                .HasDatabaseName("IX_Categories_Active");
        }
    }
}