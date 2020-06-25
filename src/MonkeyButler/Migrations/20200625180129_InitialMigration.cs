using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MonkeyButler.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FreeCompanyOptions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeCompanyOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildOptions",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    FreeCompanyId = table.Column<string>(nullable: true),
                    Prefix = table.Column<string>(nullable: true),
                    Server = table.Column<string>(nullable: true),
                    SignupEmotes = table.Column<List<string>>(nullable: true),
                    VerifiedRole = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildOptions_FreeCompanyOptions_FreeCompanyId",
                        column: x => x.FreeCompanyId,
                        principalTable: "FreeCompanyOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildOptions_FreeCompanyId",
                table: "GuildOptions",
                column: "FreeCompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildOptions");

            migrationBuilder.DropTable(
                name: "FreeCompanyOptions");
        }
    }
}
