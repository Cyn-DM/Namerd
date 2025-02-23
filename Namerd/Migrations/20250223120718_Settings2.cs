using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Namerd.Migrations
{
    /// <inheritdoc />
    public partial class Settings2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bots_MonthlyNomination_MonthlyNominationId",
                table: "Bots");

            migrationBuilder.DropForeignKey(
                name: "FK_NominationDetails_MonthlyNomination_MonthlyNominationId",
                table: "NominationDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonthlyNomination",
                table: "MonthlyNomination");

            migrationBuilder.RenameTable(
                name: "MonthlyNomination",
                newName: "MonthlyNominations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonthlyNominations",
                table: "MonthlyNominations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bots_MonthlyNominations_MonthlyNominationId",
                table: "Bots",
                column: "MonthlyNominationId",
                principalTable: "MonthlyNominations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NominationDetails_MonthlyNominations_MonthlyNominationId",
                table: "NominationDetails",
                column: "MonthlyNominationId",
                principalTable: "MonthlyNominations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bots_MonthlyNominations_MonthlyNominationId",
                table: "Bots");

            migrationBuilder.DropForeignKey(
                name: "FK_NominationDetails_MonthlyNominations_MonthlyNominationId",
                table: "NominationDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonthlyNominations",
                table: "MonthlyNominations");

            migrationBuilder.RenameTable(
                name: "MonthlyNominations",
                newName: "MonthlyNomination");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonthlyNomination",
                table: "MonthlyNomination",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bots_MonthlyNomination_MonthlyNominationId",
                table: "Bots",
                column: "MonthlyNominationId",
                principalTable: "MonthlyNomination",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NominationDetails_MonthlyNomination_MonthlyNominationId",
                table: "NominationDetails",
                column: "MonthlyNominationId",
                principalTable: "MonthlyNomination",
                principalColumn: "Id");
        }
    }
}
