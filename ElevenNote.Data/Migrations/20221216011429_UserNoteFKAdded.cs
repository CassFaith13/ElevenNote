using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElevenNote.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserNoteFKAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Notes_OwnerID",
                table: "Notes",
                column: "OwnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Users_OwnerID",
                table: "Notes",
                column: "OwnerID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Users_OwnerID",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_OwnerID",
                table: "Notes");
        }
    }
}
