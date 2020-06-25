using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MonkeyButler.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildOptions");
        }
    }
}
