using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Namerd.Migrations
{
    /// <inheritdoc />
    public partial class Settings4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyNominations_Bots_NamerdBotGuildId",
                table: "MonthlyNominations");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyNominations_NamerdBotGuildId",
                table: "MonthlyNominations");

            migrationBuilder.DropColumn(
                name: "NamerdBotGuildId",
                table: "MonthlyNominations");

            migrationBuilder.AddColumn<decimal>(
                name: "botId",
                table: "MonthlyNominations",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyNominations_botId",
                table: "MonthlyNominations",
                column: "botId");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyNominations_Bots_botId",
                table: "MonthlyNominations",
                column: "botId",
                principalTable: "Bots",
                principalColumn: "GuildId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyNominations_Bots_botId",
                table: "MonthlyNominations");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyNominations_botId",
                table: "MonthlyNominations");

            migrationBuilder.DropColumn(
                name: "botId",
                table: "MonthlyNominations");

            migrationBuilder.AddColumn<decimal>(
                name: "NamerdBotGuildId",
                table: "MonthlyNominations",
                type: "numeric(20,0)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyNominations_NamerdBotGuildId",
                table: "MonthlyNominations",
                column: "NamerdBotGuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyNominations_Bots_NamerdBotGuildId",
                table: "MonthlyNominations",
                column: "NamerdBotGuildId",
                principalTable: "Bots",
                principalColumn: "GuildId");
        }
    }
}
