using Microsoft.EntityFrameworkCore.Migrations;

namespace MonkeyButler.Migrations
{
    public partial class ChangedGuildOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildOptions_FreeCompanyOptions_FreeCompanyId",
                table: "GuildOptions");

            migrationBuilder.DropTable(
                name: "FreeCompanyOptions");

            migrationBuilder.DropIndex(
                name: "IX_GuildOptions_FreeCompanyId",
                table: "GuildOptions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FreeCompanyOptions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeCompanyOptions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildOptions_FreeCompanyId",
                table: "GuildOptions",
                column: "FreeCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildOptions_FreeCompanyOptions_FreeCompanyId",
                table: "GuildOptions",
                column: "FreeCompanyId",
                principalTable: "FreeCompanyOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
