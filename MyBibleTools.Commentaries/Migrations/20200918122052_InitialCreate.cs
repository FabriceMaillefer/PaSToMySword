using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBibleTools.Commentaries.Migrations
{
    public partial class InitialCreate : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "commentaries",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    book_number = table.Column<int>(nullable: false),
                    chapter_number_from = table.Column<int>(nullable: false),
                    verse_number_from = table.Column<int>(nullable: false),
                    chapter_number_to = table.Column<int>(nullable: false),
                    verse_number_to = table.Column<int>(nullable: false),
                    text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commentaries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "info",
                columns: table => new
                {
                    name = table.Column<string>(nullable: false),
                    value = table.Column<string>(name: "value ", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_info", x => x.name);
                });

            migrationBuilder.CreateIndex(
                name: "IX_commentaries_book_number_chapter_number_from_chapter_number_to",
                table: "commentaries",
                columns: new[] { "book_number", "chapter_number_from", "chapter_number_to" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "commentaries");

            migrationBuilder.DropTable(
                name: "info");
        }

        #endregion Methods
    }
}