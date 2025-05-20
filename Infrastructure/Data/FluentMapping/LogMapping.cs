using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping
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
                .HasDefaultValueSql("now()")
                .IsRequired();

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
                .HasColumnType("String")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(l => l.Level)
                .HasColumnName("Level")
                .HasColumnType("String")
                .HasMaxLength(20)
                .IsRequired(false);

            builder.OwnsOne(l => l.Message, msg =>
            {
                msg.Property(m => m.Text)
                    .HasColumnName("Message")
                    .HasColumnType("String")
                    .HasMaxLength(1000)
                    .IsRequired(false);
            });

            builder.OwnsOne(l => l.StackTrace, st =>
            {
                st.Property(s => s.Body)
                    .HasColumnName("StackTrace")
                    .HasColumnType("String")
                    .HasMaxLength(4000)
                    .IsRequired(false);
            });

            builder.Property(l => l.AppId)
                .HasColumnName("AppId")
                .HasColumnType("UUID")
                .IsRequired();
            // Remover relacionamento App caso ClickHouse não suporte FK
            // builder.HasOne(l => l.App)
            //     .WithMany(a => a.Logs)
            //     .HasForeignKey(l => l.AppId)
            //     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
