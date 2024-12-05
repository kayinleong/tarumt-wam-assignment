using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tarumt.WAM.Assignment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migrate_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserSecurityMetas_SecurityMetaId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserSecurityMetas");

            migrationBuilder.DropIndex(
                name: "IX_Users_SecurityMetaId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityMetaId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "LoginAttempt",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamps",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginAttempt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityStamps",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "SecurityMetaId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserSecurityMetas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoginAttempt = table.Column<int>(type: "int", nullable: false),
                    SecurityStamps = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSecurityMetas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_SecurityMetaId",
                table: "Users",
                column: "SecurityMetaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserSecurityMetas_SecurityMetaId",
                table: "Users",
                column: "SecurityMetaId",
                principalTable: "UserSecurityMetas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
