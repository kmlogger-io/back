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
                .HasColumnType("UUID")
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(p => p.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("DateTime")
                .HasDefaultValueSql("now()")
                .IsRequired();
            builder.Property(p => p.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("DateTime")
                .HasDefaultValueSql("now()")
                .IsRequired();
            builder.Property(p => p.DeletedDate)
                .HasColumnName("DeletedDate")
                .HasColumnType("DateTime")
                .IsRequired(false);
            builder.OwnsOne(p => p.Name, name =>
            {
                name.Property(n => n.Name)
                    .HasColumnName("Name")
                    .HasColumnType("String")
                    .HasMaxLength(100)
                    .IsRequired();
            });
            builder.Property(p => p.AwsKey)
                .HasColumnName("AwsKey")
                .HasColumnType("String")
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(p => p.UrlExpired)
                .HasColumnName("UrlExpired")
                .HasColumnType("DateTime")
                .IsRequired();
            builder.Property(p => p.UrlTemp)
                .HasColumnName("UrlTemp")
                .HasColumnType("String")
                .HasMaxLength(300)
                .IsRequired();
            builder.Property(p => p.Ativo)
                .HasColumnName("Ativo")
                .HasColumnType("Bool")
                .IsRequired();
        }
    }
}
