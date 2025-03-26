using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace todo_app_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseV9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TempUserId",
                table: "TodoTask",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TempUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoTask_TempUserId",
                table: "TodoTask",
                column: "TempUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTask_TempUser_TempUserId",
                table: "TodoTask",
                column: "TempUserId",
                principalTable: "TempUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTask_TempUser_TempUserId",
                table: "TodoTask");

            migrationBuilder.DropTable(
                name: "TempUser");

            migrationBuilder.DropIndex(
                name: "IX_TodoTask_TempUserId",
                table: "TodoTask");

            migrationBuilder.DropColumn(
                name: "TempUserId",
                table: "TodoTask");
        }
    }
}
