using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping
{
    public class RoleMapping : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            
            builder.HasKey(r => r.Id);
            
            builder.Property(r => r.Id)
                .HasColumnName("Id")
                .HasColumnType("uuid")
                .ValueGeneratedOnAdd()
                .IsRequired();
            
            builder.Property(r => r.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();
            
            builder.Property(r => r.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();
            
            builder.Property(r => r.DeletedDate)
                .HasColumnName("DeletedDate")
                .HasColumnType("timestamptz")
                .IsRequired(false);
            
            // ValueObject UniqueName
            builder.OwnsOne(r => r.Name, name =>
            {
                name.Property(n => n.Name)
                    .HasColumnName("RoleName")
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100)
                    .IsRequired();
                
                // Índice único no nome da role
                name.HasIndex(n => n.Name)
                    .IsUnique()
                    .HasDatabaseName("IX_Roles_Name_Unique");
            });
            
            builder.Property(r => r.Slug)
                .HasColumnName("Slug")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();
            
            // Índices para performance
            builder.HasIndex(r => r.CreatedDate)
                .HasDatabaseName("IX_Roles_CreatedDate");
            
            builder.HasIndex(r => r.Slug)
                .IsUnique()
                .HasDatabaseName("IX_Roles_Slug_Unique");
        }
    }
}