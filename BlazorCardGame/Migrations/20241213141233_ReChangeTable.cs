using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorCardGame.Migrations
{
    /// <inheritdoc />
    public partial class ReChangeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DicardsCardCount",
                table: "Games",
                newName: "DiscardCardsCount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiscardCardsCount",
                table: "Games",
                newName: "DicardsCardCount");
        }
    }
}
