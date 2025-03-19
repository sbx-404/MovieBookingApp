using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class excludeGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Movies_MovieDatabaseId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_MovieDatabaseId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "MovieDatabaseId",
                table: "Genres");

            migrationBuilder.RenameColumn(
                name: "vote_average",
                table: "Movies",
                newName: "Vote_average");

            migrationBuilder.RenameColumn(
                name: "runtime",
                table: "Movies",
                newName: "Runtime");

            migrationBuilder.RenameColumn(
                name: "release_date",
                table: "Movies",
                newName: "Release_date");

            migrationBuilder.RenameColumn(
                name: "poster_path",
                table: "Movies",
                newName: "Poster_path");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Vote_average",
                table: "Movies",
                newName: "vote_average");

            migrationBuilder.RenameColumn(
                name: "Runtime",
                table: "Movies",
                newName: "runtime");

            migrationBuilder.RenameColumn(
                name: "Release_date",
                table: "Movies",
                newName: "release_date");

            migrationBuilder.RenameColumn(
                name: "Poster_path",
                table: "Movies",
                newName: "poster_path");

            migrationBuilder.AddColumn<int>(
                name: "MovieDatabaseId",
                table: "Genres",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_MovieDatabaseId",
                table: "Genres",
                column: "MovieDatabaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Movies_MovieDatabaseId",
                table: "Genres",
                column: "MovieDatabaseId",
                principalTable: "Movies",
                principalColumn: "DatabaseId");
        }
    }
}
