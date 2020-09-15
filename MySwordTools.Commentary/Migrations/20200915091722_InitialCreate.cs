using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MySwordTools.Commentaries.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "commentary",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    book = table.Column<int>(nullable: true),
                    chapter = table.Column<int>(nullable: true),
                    fromverse = table.Column<int>(nullable: true),
                    toverse = table.Column<int>(nullable: true),
                    data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commentary", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "details",
                columns: table => new
                {
                    title = table.Column<string>(nullable: false),
                    abbreviation = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    comments = table.Column<string>(nullable: true),
                    author = table.Column<string>(nullable: true),
                    version = table.Column<string>(nullable: true),
                    versiondate = table.Column<DateTime>(nullable: true),
                    publishdate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_details", x => x.title);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "commentary");

            migrationBuilder.DropTable(
                name: "details");
        }
    }
}
