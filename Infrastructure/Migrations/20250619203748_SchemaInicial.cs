using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SchemaInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    AwsKey = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    UrlExpired = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    UrlTemp = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    EmailAddress = table.Column<string>(type: "varchar(254)", maxLength: 254, nullable: false),
                    AddressRoad = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    AddressNeighborHood = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    AddressNumber = table.Column<long>(type: "bigint", nullable: true),
                    AddressComplement = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PasswordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    PasswordSalt = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TokenActivate = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Apps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Environment = table.Column<string>(type: "varchar(50)", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apps_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false),
                    RolesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UsersId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_UserRole_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Environment = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Level = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    StackTrace = table.Column<string>(type: "text", nullable: true),
                    AppId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    DeletedDate = table.Column<DateTime>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Apps_AppId",
                        column: x => x.AppId,
                        principalTable: "Apps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apps_Active",
                table: "Apps",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_Apps_CategoryId",
                table: "Apps",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Apps_CreatedDate",
                table: "Apps",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Active",
                table: "Categories",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatedDate",
                table: "Categories",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name_Unique",
                table: "Categories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_AppId",
                table: "Logs",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_AppId_CreatedDate_Level",
                table: "Logs",
                columns: new[] { "AppId", "CreatedDate", "Level" });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_CreatedDate_Level",
                table: "Logs",
                columns: new[] { "CreatedDate", "Level" });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Environment",
                table: "Logs",
                column: "Environment");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Level",
                table: "Logs",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_Ativo",
                table: "Pictures",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_AwsKey_Unique",
                table: "Pictures",
                column: "AwsKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_CreatedDate",
                table: "Pictures",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_UrlExpired",
                table: "Pictures",
                column: "UrlExpired");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_UrlExpired_Ativo",
                table: "Pictures",
                columns: new[] { "UrlExpired", "Ativo" });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedDate",
                table: "Roles",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name_Unique",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Slug_Unique",
                table: "Roles",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RolesId",
                table: "UserRole",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Active",
                table: "Users",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedDate",
                table: "Users",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_Active",
                table: "Users",
                column: "EmailAddress",
                unique: true,
                filter: "\"Active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FullName",
                table: "Users",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TokenActivate",
                table: "Users",
                column: "TokenActivate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Apps");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
