using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Environment = Domain.Enums.Environment;

namespace Infrastructure.Data.Hot.FluentMapping
{
    public class LogAppMapping : IEntityTypeConfiguration<LogApp>
    {
        public void Configure(EntityTypeBuilder<LogApp> builder)
        {
            // Nome da tabela
            builder.ToTable("Logs");

            // Chave primária
            builder.HasKey(l => l.Id);

            // Propriedades
            builder.Property(l => l.Id)
                .HasColumnName("Id")
                .HasColumnType("UUID")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(l => l.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("DateTime")
                .HasDefaultValueSql("now()");

            builder.Property(l => l.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("DateTime")
                .IsRequired();

            builder.Property(l => l.DeletedDate)
                .HasColumnName("DeletedDate")
                .HasColumnType("DateTime")
                .IsRequired(false);

            builder.Property(l => l.Environment)
                .HasColumnName("Environment")
                .HasConversion<string>();

            builder.Property(l => l.Level)
                .HasColumnName("Level")
                .HasConversion<string>();
            // Mapeamento do Value Object Description para Message
            builder.OwnsOne(l => l.Message, msg =>
            {
                msg.Property(m => m.Text)
                    .HasColumnName("Message")
                    .HasColumnType("String");
            });

            // Mapeamento do Value Object Description para StackTrace
            builder.OwnsOne(l => l.StackTrace, st =>
            {
                st.Property(s => s.Body)
                    .HasColumnName("StackTrace")
                    .HasColumnType("String");
            });

            builder.HasOne(l => l.App)
                .WithMany(a => a.Logs)
                .HasForeignKey(l => l.AppId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
