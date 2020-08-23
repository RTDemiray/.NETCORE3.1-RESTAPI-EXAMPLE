using Microsoft.EntityFrameworkCore.Migrations;

namespace FlutterApp.Data.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Scores");

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Scores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scores_UsersId",
                table: "Scores",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Users_UsersId",
                table: "Scores",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Users_UsersId",
                table: "Scores");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Scores_UsersId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Scores");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Scores",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");
        }
    }
}
