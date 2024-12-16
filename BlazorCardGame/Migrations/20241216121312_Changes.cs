using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorCardGame.Migrations
{
    /// <inheritdoc />
    public partial class Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardInfo_Games_FoolGamePlayerInfoName_FoolGameOpponentInfoNa~",
                table: "CardInfo");

            migrationBuilder.DropColumn(
                name: "FoolGameId",
                table: "CardInfo");

            migrationBuilder.AlterColumn<string>(
                name: "FoolGamePlayerInfoName",
                table: "CardInfo",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "FoolGameOpponentInfoName",
                table: "CardInfo",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_Games_FoolGamePlayerInfoName_FoolGameOpponentInfoNa~",
                table: "CardInfo",
                columns: new[] { "FoolGamePlayerInfoName", "FoolGameOpponentInfoName" },
                principalTable: "Games",
                principalColumns: new[] { "PlayerInfoName", "OpponentInfoName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardInfo_Games_FoolGamePlayerInfoName_FoolGameOpponentInfoNa~",
                table: "CardInfo");

            migrationBuilder.UpdateData(
                table: "CardInfo",
                keyColumn: "FoolGamePlayerInfoName",
                keyValue: null,
                column: "FoolGamePlayerInfoName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "FoolGamePlayerInfoName",
                table: "CardInfo",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CardInfo",
                keyColumn: "FoolGameOpponentInfoName",
                keyValue: null,
                column: "FoolGameOpponentInfoName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "FoolGameOpponentInfoName",
                table: "CardInfo",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "FoolGameId",
                table: "CardInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_Games_FoolGamePlayerInfoName_FoolGameOpponentInfoNa~",
                table: "CardInfo",
                columns: new[] { "FoolGamePlayerInfoName", "FoolGameOpponentInfoName" },
                principalTable: "Games",
                principalColumns: new[] { "PlayerInfoName", "OpponentInfoName" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
