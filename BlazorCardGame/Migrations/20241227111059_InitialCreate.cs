using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorCardGame.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    PlayerInfoName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OpponentInfoName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiscardCardsCount = table.Column<int>(type: "int", nullable: false),
                    CountOfGames = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => new { x.PlayerInfoName, x.OpponentInfoName });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Login = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastEnteredTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsLoggedIn = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Login);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CardInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CardType = table.Column<int>(type: "int", nullable: false),
                    FoolGameOpponentInfoName = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FoolGamePlayerInfoName = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardInfo_Games_FoolGamePlayerInfoName_FoolGameOpponentInfoNa~",
                        columns: x => new { x.FoolGamePlayerInfoName, x.FoolGameOpponentInfoName },
                        principalTable: "Games",
                        principalColumns: new[] { "PlayerInfoName", "OpponentInfoName" });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserLogin = table.Column<string>(type: "varchar(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PlayerType = table.Column<int>(type: "int", nullable: false),
                    IsAttack = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Players_Users_UserLogin",
                        column: x => x.UserLogin,
                        principalTable: "Users",
                        principalColumn: "Login");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerInfoName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NumberOfWins = table.Column<int>(type: "int", nullable: true),
                    NumberOfLosses = table.Column<int>(type: "int", nullable: true),
                    NumberOfDraws = table.Column<int>(type: "int", nullable: true),
                    CountOfGames = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scores_Players_PlayerInfoName",
                        column: x => x.PlayerInfoName,
                        principalTable: "Players",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_FoolGamePlayerInfoName_FoolGameOpponentInfoName",
                table: "CardInfo",
                columns: new[] { "FoolGamePlayerInfoName", "FoolGameOpponentInfoName" });

            migrationBuilder.CreateIndex(
                name: "IX_Players_UserLogin",
                table: "Players",
                column: "UserLogin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scores_PlayerInfoName",
                table: "Scores",
                column: "PlayerInfoName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardInfo");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
