using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlutterApp.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowGuid = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowGuid = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Question = table.Column<string>(maxLength: 200, nullable: false),
                    OptionA = table.Column<string>(nullable: false),
                    OptionB = table.Column<string>(nullable: false),
                    OptionC = table.Column<string>(nullable: false),
                    OptionD = table.Column<string>(nullable: false),
                    OptionE = table.Column<string>(nullable: false),
                    TrueOption = table.Column<string>(nullable: false),
                    CategoriesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowGuid = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Rank = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(maxLength: 60, nullable: false),
                    CategoriesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scores_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CategoriesId",
                table: "Questions",
                column: "CategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_CategoriesId",
                table: "Scores",
                column: "CategoriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
