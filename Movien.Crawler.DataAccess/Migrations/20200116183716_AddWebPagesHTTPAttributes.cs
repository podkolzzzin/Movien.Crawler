using Microsoft.EntityFrameworkCore.Migrations;

namespace Movien.Crawler.DataAccess.Migrations
{
    public partial class AddWebPagesHTTPAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HttpMethod",
                table: "Pages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Pages",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Headers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WebPageId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Headers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Headers_Pages_WebPageId",
                        column: x => x.WebPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WebPageId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parameters_Pages_WebPageId",
                        column: x => x.WebPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Headers_WebPageId",
                table: "Headers",
                column: "WebPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_WebPageId",
                table: "Parameters",
                column: "WebPageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Headers");

            migrationBuilder.DropTable(
                name: "Parameters");

            migrationBuilder.DropColumn(
                name: "HttpMethod",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Pages");
        }
    }
}
