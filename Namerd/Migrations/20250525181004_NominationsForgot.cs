using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Namerd.Migrations
{
    /// <inheritdoc />
    public partial class NominationsForgot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NominationReason",
                table: "Nominations",
                type: "character varying(280)",
                maxLength: 280,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NominationReason",
                table: "Nominations");
        }
    }
}
