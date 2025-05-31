using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Namerd.Migrations
{
    /// <inheritdoc />
    public partial class Nominations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NominationDetails");

            migrationBuilder.DropTable(
                name: "MonthlyNominations");

            migrationBuilder.AddColumn<DateTime>(
                name: "NominateEndTimeUTC",
                table: "Settings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "NominationPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    botId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NominationPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NominationPeriods_Bots_botId",
                        column: x => x.botId,
                        principalTable: "Bots",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nominations",
                columns: table => new
                {
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    NominationPeriodId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nominations", x => new { x.UserId, x.NominationPeriodId });
                    table.ForeignKey(
                        name: "FK_Nominations_NominationPeriods_NominationPeriodId",
                        column: x => x.NominationPeriodId,
                        principalTable: "NominationPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NominationPeriods_botId",
                table: "NominationPeriods",
                column: "botId");

            migrationBuilder.CreateIndex(
                name: "IX_Nominations_NominationPeriodId",
                table: "Nominations",
                column: "NominationPeriodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nominations");

            migrationBuilder.DropTable(
                name: "NominationPeriods");

            migrationBuilder.DropColumn(
                name: "NominateEndTimeUTC",
                table: "Settings");

            migrationBuilder.CreateTable(
                name: "MonthlyNominations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    botId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsNominationActive = table.Column<bool>(type: "boolean", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyNominations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyNominations_Bots_botId",
                        column: x => x.botId,
                        principalTable: "Bots",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NominationDetails",
                columns: table => new
                {
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    MonthlyNominationId = table.Column<int>(type: "integer", nullable: true),
                    Reason = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Votes = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NominationDetails", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_NominationDetails_MonthlyNominations_MonthlyNominationId",
                        column: x => x.MonthlyNominationId,
                        principalTable: "MonthlyNominations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyNominations_botId",
                table: "MonthlyNominations",
                column: "botId");

            migrationBuilder.CreateIndex(
                name: "IX_NominationDetails_MonthlyNominationId",
                table: "NominationDetails",
                column: "MonthlyNominationId");
        }
    }
}
