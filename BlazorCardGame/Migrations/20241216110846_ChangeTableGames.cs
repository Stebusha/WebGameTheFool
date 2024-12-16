using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorCardGame.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardInfo_Games_FoolGameId",
                table: "CardInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_OpponentInfoName",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_PlayerInfoName",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_OpponentInfoName",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_PlayerInfoName",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_CardInfo_FoolGameId",
                table: "CardInfo");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "FoolGameOpponentInfoName",
                table: "CardInfo",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FoolGamePlayerInfoName",
                table: "CardInfo",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                columns: new[] { "PlayerInfoName", "OpponentInfoName" });

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_FoolGamePlayerInfoName_FoolGameOpponentInfoName",
                table: "CardInfo",
                columns: new[] { "FoolGamePlayerInfoName", "FoolGameOpponentInfoName" });

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_Games_FoolGamePlayerInfoName_FoolGameOpponentInfoNa~",
                table: "CardInfo",
                columns: new[] { "FoolGamePlayerInfoName", "FoolGameOpponentInfoName" },
                principalTable: "Games",
                principalColumns: new[] { "PlayerInfoName", "OpponentInfoName" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardInfo_Games_FoolGamePlayerInfoName_FoolGameOpponentInfoNa~",
                table: "CardInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_CardInfo_FoolGamePlayerInfoName_FoolGameOpponentInfoName",
                table: "CardInfo");

            migrationBuilder.DropColumn(
                name: "FoolGameOpponentInfoName",
                table: "CardInfo");

            migrationBuilder.DropColumn(
                name: "FoolGamePlayerInfoName",
                table: "CardInfo");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_OpponentInfoName",
                table: "Games",
                column: "OpponentInfoName");

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlayerInfoName",
                table: "Games",
                column: "PlayerInfoName");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_FoolGameId",
                table: "CardInfo",
                column: "FoolGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_Games_FoolGameId",
                table: "CardInfo",
                column: "FoolGameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_OpponentInfoName",
                table: "Games",
                column: "OpponentInfoName",
                principalTable: "Players",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_PlayerInfoName",
                table: "Games",
                column: "PlayerInfoName",
                principalTable: "Players",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
