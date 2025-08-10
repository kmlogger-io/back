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
                .HasColumnType("uuid")
                .ValueGeneratedOnAdd()
                .IsRequired();
            
            builder.Property(a => a.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();
            
            builder.Property(a => a.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();
            
            builder.Property(a => a.DeletedDate)
                .HasColumnName("DeletedDate")
                .HasColumnType("timestamptz")
                .IsRequired(false);
            
            builder.Property(a => a.Environment)
                .HasColumnName("Environment")
                .HasConversion<string>()
                .HasColumnType("varchar(50)")
                .IsRequired(false);
            
            builder.Property(a => a.Active)
                .HasColumnName("Active")
                .HasColumnType("boolean")
                .HasDefaultValue(true)
                .IsRequired(false);
            
            builder.OwnsOne(a => a.Name, name =>
            {
                name.Property(n => n.Name)
                    .HasColumnName("Name")
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100)
                    .IsRequired();
            });
            
            builder.Property(a => a.CategoryId)
                .HasColumnName("CategoryId")
                .HasColumnType("uuid")
                .IsRequired();
            
            builder.HasIndex(a => a.CreatedDate)
                .HasDatabaseName("IX_Apps_CreatedDate");
            
            builder.HasIndex(a => a.Active)
                .HasDatabaseName("IX_Apps_Active");
            
            builder.HasIndex(a => a.CategoryId)
                .HasDatabaseName("IX_Apps_CategoryId");
        }
    }
}