using Microsoft.EntityFrameworkCore.Migrations;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Security.Cryptography;
using System.Text;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var adminRoleId = Guid.NewGuid().ToString();
            var userRoleId = Guid.NewGuid().ToString();
            var moderatorRoleId = Guid.NewGuid().ToString();
            var adminUserId = Guid.NewGuid().ToString();

            // 1. Inserir Roles com IDs gerados
            migrationBuilder.Sql($@"
                INSERT INTO ""Roles"" (""Id"", ""CreatedDate"", ""UpdatedDate"", ""RoleName"", ""Slug"")
                SELECT
                    '{adminRoleId}'::uuid,
                    NOW(),
                    NOW(),
                    'Administrator',
                    'admin'
                WHERE NOT EXISTS (
                    SELECT 1 FROM ""Roles"" WHERE ""Slug"" = 'admin'
                );

                INSERT INTO ""Roles"" (""Id"", ""CreatedDate"", ""UpdatedDate"", ""RoleName"", ""Slug"")
                SELECT
                    '{userRoleId}'::uuid,
                    NOW(),
                    NOW(),
                    'User',
                    'user'
                WHERE NOT EXISTS (
                    SELECT 1 FROM ""Roles"" WHERE ""Slug"" = 'user'
                );

                INSERT INTO ""Roles"" (""Id"", ""CreatedDate"", ""UpdatedDate"", ""RoleName"", ""Slug"")
                SELECT
                    '{moderatorRoleId}'::uuid,
                    NOW(),
                    NOW(),
                    'Moderator',
                    'moderator'
                WHERE NOT EXISTS (
                    SELECT 1 FROM ""Roles"" WHERE ""Slug"" = 'moderator'
                );
            ");

            // 2. Gerar hash da senha 'admin'
            var salt = GenerateSalt();
            var passwordHash = HashPassword("admin", salt);

            // 3. Inserir usuário admin com ID gerado
            migrationBuilder.Sql($@"
                INSERT INTO ""Users"" (
                    ""Id"", 
                    ""CreatedDate"", 
                    ""UpdatedDate"", 
                    ""FirstName"", 
                    ""LastName"", 
                    ""EmailAddress"", 
                    ""PasswordHash"", 
                    ""PasswordSalt"", 
                    ""Active"", 
                    ""TokenActivate"",
                    ""AddressRoad"",
                    ""AddressNumber"",
                    ""AddressNeighborHood"",
                    ""AddressComplement""
                )
                SELECT
                    '{adminUserId}'::uuid,
                    NOW(),
                    NOW(),
                    'Admin',
                    'System',
                    'admin@kmlogger.com',
                    '{passwordHash}',
                    '{salt}',
                    true,
                    null,
                    null,
                    null,
                    null,
                    null
                WHERE NOT EXISTS (
                    SELECT 1 FROM ""Users"" WHERE ""EmailAddress"" = 'admin@kmlogger.com'
                );
            ");

            // 4. Criar tabela UserRole (se não existir)
            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS ""UserRole"" (
                    ""UsersId"" uuid NOT NULL,
                    ""RolesId"" uuid NOT NULL,
                    PRIMARY KEY (""UsersId"", ""RolesId""),
                    CONSTRAINT ""FK_UserRole_Users_UsersId"" FOREIGN KEY (""UsersId"") REFERENCES ""Users""(""Id"") ON DELETE CASCADE,
                    CONSTRAINT ""FK_UserRole_Roles_RolesId"" FOREIGN KEY (""RolesId"") REFERENCES ""Roles""(""Id"") ON DELETE CASCADE
                );

                CREATE INDEX IF NOT EXISTS ""IX_UserRole_RolesId"" ON ""UserRole""(""RolesId"");
            ");

            // 5. Associar usuário admin à role admin usando os IDs gerados
            migrationBuilder.Sql($@"
                INSERT INTO ""UserRole"" (""UsersId"", ""RolesId"")
                SELECT
                    '{adminUserId}'::uuid,
                    '{adminRoleId}'::uuid
                WHERE NOT EXISTS (
                    SELECT 1 FROM ""UserRole"" 
                    WHERE ""UsersId"" = '{adminUserId}'::uuid 
                    AND ""RolesId"" = '{adminRoleId}'::uuid
                ) AND EXISTS (
                    SELECT 1 FROM ""Users"" WHERE ""Id"" = '{adminUserId}'::uuid
                ) AND EXISTS (
                    SELECT 1 FROM ""Roles"" WHERE ""Id"" = '{adminRoleId}'::uuid
                );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover dados do usuário admin e roles
            migrationBuilder.Sql(@"
                DELETE FROM ""UserRole"" 
                WHERE ""UsersId"" IN (
                    SELECT ""Id"" FROM ""Users"" WHERE ""EmailAddress"" = 'admin@kmlogger.com'
                );
                
                DELETE FROM ""Users"" 
                WHERE ""EmailAddress"" = 'admin@kmlogger.com';
                
                DELETE FROM ""Roles"" 
                WHERE ""Slug"" IN ('admin', 'user', 'moderator');
                
                DROP TABLE IF EXISTS ""UserRole"";
            ");
        }

        // Métodos auxiliares para hash da senha
        private static string GenerateSalt()
        {
            var bytes = new byte[16]; // 16 bytes = 128 bits (compatível com seu ValueObject)
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(password + salt);
            var hashBytes = sha256.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}