using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieStore.Infrastructure.Migrations
{
    public partial class CreatingFavoriteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Favorite_MovieId_UserId",
                table: "Favorite");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_MovieId",
                table: "Favorite",
                column: "MovieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Favorite_MovieId",
                table: "Favorite");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_MovieId_UserId",
                table: "Favorite",
                columns: new[] { "MovieId", "UserId" },
                unique: true);
        }
    }
}
