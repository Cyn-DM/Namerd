using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Namerd.Migrations
{
    /// <inheritdoc />
    public partial class Settings3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bots_MonthlyNominations_MonthlyNominationId",
                table: "Bots");

            migrationBuilder.DropIndex(
                name: "IX_Bots_MonthlyNominationId",
                table: "Bots");

            migrationBuilder.DropColumn(
                name: "MonthlyNominationId",
                table: "Bots");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "MonthlyNominationId",
                table: "Bots",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bots_MonthlyNominationId",
                table: "Bots",
                column: "MonthlyNominationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bots_MonthlyNominations_MonthlyNominationId",
                table: "Bots",
                column: "MonthlyNominationId",
                principalTable: "MonthlyNominations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
