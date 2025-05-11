using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Cold.FluentMapping;

public class LogAppColdMapping : IEntityTypeConfiguration<LogApp>
{
    public void Configure(EntityTypeBuilder<LogApp> builder)
    {
        builder.ToTable("Logs");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasColumnName("Id")
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(l => l.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()");

        builder.Property(l => l.UpdatedDate)
            .HasColumnName("UpdatedDate")
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(l => l.DeletedDate)
            .HasColumnName("DeletedDate")
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder.Property(l => l.Environment)
            .HasColumnName("Environment")
            .HasConversion<string>(); 

        builder.Property(l => l.Level)
            .HasColumnName("Level")
            .HasConversion<string>(); 

        builder.OwnsOne(l => l.Message, msg =>
        {
            msg.Property(m => m.Text)
                .HasColumnName("Message")
                .HasColumnType("text");
        });

        builder.OwnsOne(l => l.StackTrace, st =>
        {
            st.Property(s => s.Body)
                .HasColumnName("StackTrace")
                .HasColumnType("text");
        });

        builder.HasOne(l => l.App)
            .WithMany(a => a.Logs)
            .HasForeignKey(l => l.AppId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
