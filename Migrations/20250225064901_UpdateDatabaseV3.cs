using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace todo_app_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTaskTag_Tag_TodoTaskId",
                table: "TodoTaskTag");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoTaskTag_TodoTask_TagId",
                table: "TodoTaskTag");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTaskTag_Tag_TagId",
                table: "TodoTaskTag",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTaskTag_TodoTask_TodoTaskId",
                table: "TodoTaskTag",
                column: "TodoTaskId",
                principalTable: "TodoTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTaskTag_Tag_TagId",
                table: "TodoTaskTag");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoTaskTag_TodoTask_TodoTaskId",
                table: "TodoTaskTag");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTaskTag_Tag_TodoTaskId",
                table: "TodoTaskTag",
                column: "TodoTaskId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTaskTag_TodoTask_TagId",
                table: "TodoTaskTag",
                column: "TagId",
                principalTable: "TodoTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
