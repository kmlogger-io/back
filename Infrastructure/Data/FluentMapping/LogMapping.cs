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
                .HasColumnType("uuid")
                .ValueGeneratedOnAdd()
                .IsRequired();
            
            builder.Property(l => l.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();
            
            builder.Property(l => l.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();
            
            builder.Property(l => l.DeletedDate)
                .HasColumnName("DeletedDate")
                .HasColumnType("timestamptz")
                .IsRequired(false);
            
            builder.Property(l => l.Environment)
                .HasColumnName("Environment")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();
            
            builder.Property(l => l.Level)
                .HasColumnName("Level")
                .HasColumnType("varchar(20)")
                .HasMaxLength(20)
                .IsRequired(false);
            
            builder.OwnsOne(l => l.Message, msg =>
            {
                msg.Property(m => m.Text)
                    .HasColumnName("Message")
                    .HasColumnType("text")
                    .IsRequired(false);
            });
            
            builder.Navigation(l => l.Message).IsRequired(false);
            
            builder.OwnsOne(l => l.StackTrace, st =>
            {
                st.Property(s => s.Body)
                    .HasColumnName("StackTrace")
                    .HasColumnType("text")
                    .IsRequired(false);
            });
            
            builder.Navigation(l => l.StackTrace).IsRequired(false);
            
            builder.Property(l => l.AppId)
                .HasColumnName("AppId")
                .HasColumnType("uuid")
                .IsRequired();
            
            builder.HasOne(l => l.App)
                .WithMany(a => a.Logs)
                .HasForeignKey(l => l.AppId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasIndex(l => new { l.CreatedDate, l.Level })
                .HasDatabaseName("IX_Logs_CreatedDate_Level");
            
            builder.HasIndex(l => l.AppId)
                .HasDatabaseName("IX_Logs_AppId");
            
            builder.HasIndex(l => l.Environment)
                .HasDatabaseName("IX_Logs_Environment");
            
            builder.HasIndex(l => l.Level)
                .HasDatabaseName("IX_Logs_Level");
            
            // Índice composto para consultas típicas de logs
            builder.HasIndex(l => new { l.AppId, l.CreatedDate, l.Level })
                .HasDatabaseName("IX_Logs_AppId_CreatedDate_Level");
        }
    }
}