using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping
{
    public class PictureMapping : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.ToTable("Pictures");
            
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.Id)
                .HasColumnName("Id")
                .HasColumnType("uuid")
                .ValueGeneratedOnAdd()
                .IsRequired();
            
            builder.Property(p => p.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();
            
            builder.Property(p => p.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();
            
            builder.Property(p => p.DeletedDate)
                .HasColumnName("DeletedDate")
                .HasColumnType("timestamptz")
                .IsRequired(false);
            
            builder.Property(p => p.Content)
                .HasColumnName("Content")
                .HasColumnType("text");

            builder.OwnsOne(p => p.Name, name =>
            {
                name.Property(n => n.Name)
                    .HasColumnName("Name")
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100)
                    .IsRequired();
            });
            
            builder.Property(p => p.AwsKey)
                .HasColumnName("AwsKey")
                .HasColumnType("varchar(200)")
                .HasMaxLength(200)
                .IsRequired();
            
            builder.Property(p => p.UrlExpired)
                .HasColumnName("UrlExpired")
                .HasColumnType("timestamptz")
                .IsRequired();
            
            builder.Property(p => p.UrlTemp)
                .HasColumnName("UrlTemp")
                .HasColumnType("varchar(500)")
                .HasMaxLength(500)
                .IsRequired();
            
            builder.Property(p => p.Ativo)
                .HasColumnName("Ativo")
                .HasColumnType("boolean")
                .HasDefaultValue(true)
                .IsRequired();
            
            // Índices para performance
            builder.HasIndex(p => p.CreatedDate)
                .HasDatabaseName("IX_Pictures_CreatedDate");
            
            builder.HasIndex(p => p.Ativo)
                .HasDatabaseName("IX_Pictures_Ativo");
            
            builder.HasIndex(p => p.UrlExpired)
                .HasDatabaseName("IX_Pictures_UrlExpired");
            
            // Índice único no AwsKey para evitar duplicação
            builder.HasIndex(p => p.AwsKey)
                .IsUnique()
                .HasDatabaseName("IX_Pictures_AwsKey_Unique");
            
            // Índice composto para limpeza de URLs expiradas
            builder.HasIndex(p => new { p.UrlExpired, p.Ativo })
                .HasDatabaseName("IX_Pictures_UrlExpired_Ativo");
        }
    }
}