using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            
            builder.HasKey(u => u.Id);
            
            builder.Property(u => u.Id)
                .HasColumnName("Id")
                .HasColumnType("uuid")
                .ValueGeneratedOnAdd()
                .IsRequired();
            
            builder.Property(u => u.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();
            
            builder.Property(u => u.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();
            
            builder.Property(u => u.DeletedDate)
                .HasColumnName("DeletedDate")
                .HasColumnType("timestamptz")
                .IsRequired(false);
            
            // FullName ValueObject
            builder.OwnsOne(u => u.FullName, fullName =>
            {
                fullName.Property(f => f.FirstName)
                    .HasColumnName("FirstName")
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100)
                    .IsRequired();
                
                fullName.Property(f => f.LastName)
                    .HasColumnName("LastName")
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100)
                    .IsRequired();
                
                // Índice para busca por nome completo
                fullName.HasIndex(f => new { f.FirstName, f.LastName })
                    .HasDatabaseName("IX_Users_FullName");
            });
            
            // Email ValueObject
            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Address)
                    .HasColumnName("EmailAddress")  
                    .HasColumnType("varchar(254)")
                    .HasMaxLength(254)
                    .IsRequired();
                
                // Índice único no email
                email.HasIndex(e => e.Address)
                    .IsUnique()
                    .HasDatabaseName("IX_Users_Email_Unique");
                
                // Índice parcial para usuários ativos
                email.HasIndex(e => e.Address)
                    .HasFilter("\"Active\" = true")
                    .HasDatabaseName("IX_Users_Email_Active");
            });
            
            // Address ValueObject
            builder.OwnsOne(u => u.Address, address =>
            {
                address.Property(a => a.Road)
                    .HasColumnName("AddressRoad") 
                    .HasColumnType("varchar(200)")
                    .HasMaxLength(200)
                    .IsRequired(false);
                
                address.Property(a => a.Number)
                    .HasColumnName("AddressNumber")  
                    .HasColumnType("bigint")
                    .IsRequired(false);
                
                address.Property(a => a.NeighBordHood)
                    .HasColumnName("AddressNeighborHood")  
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100)
                    .IsRequired(false);
                
                address.Property(a => a.Complement)
                    .HasColumnName("AddressComplement")  
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100)
                    .IsRequired(false);
            });
            
            builder.Property(u => u.TokenActivate)
                .HasColumnName("TokenActivate")
                .HasColumnType("uuid")
                .IsRequired(false);
            
            builder.Property(u => u.Active)
                .HasColumnName("Active")
                .HasColumnType("boolean")
                .HasDefaultValue(false)
                .IsRequired();
            
            // Password ValueObject
            builder.OwnsOne(u => u.Password, password =>
            {
                password.Property(p => p.Hash)
                    .HasColumnName("PasswordHash")  
                    .HasColumnType("varchar(255)")
                    .HasMaxLength(255)
                    .IsRequired(false);
                
                password.Property(p => p.Salt)
                    .HasColumnName("PasswordSalt")  
                    .HasColumnType("varchar(255)")
                    .HasMaxLength(255)
                    .IsRequired(false);
                
                password.Ignore(p => p.Content);
            });
            
            // Índices principais
            builder.HasIndex(u => u.CreatedDate)
                .HasDatabaseName("IX_Users_CreatedDate");
            
            builder.HasIndex(u => u.Active)
                .HasDatabaseName("IX_Users_Active");
            
            builder.HasIndex(u => u.TokenActivate)
                .HasDatabaseName("IX_Users_TokenActivate");
            
            builder.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(
                    "UserRole",
                    l => l.HasOne(typeof(Role)).WithMany().HasForeignKey("RolesId"),
                    r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UsersId"),
                    je =>
                    {
                        je.HasKey("UsersId", "RolesId");
                        je.HasIndex("RolesId").HasDatabaseName("IX_UserRole_RolesId");
                    });
        }
    }
}