using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreetingService.Infrastructure.Migrations
{
    public partial class CreatedRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "To",
                table: "Greetings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "From",
                table: "Greetings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Greetings_From",
                table: "Greetings",
                column: "From");

            migrationBuilder.CreateIndex(
                name: "IX_Greetings_To",
                table: "Greetings",
                column: "To");

            migrationBuilder.AddForeignKey(
                name: "FK_Greetings_Users_From",
                table: "Greetings",
                column: "From",
                principalTable: "Users",
                principalColumn: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_Greetings_Users_To",
                table: "Greetings",
                column: "To",
                principalTable: "Users",
                principalColumn: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Greetings_Users_From",
                table: "Greetings");

            migrationBuilder.DropForeignKey(
                name: "FK_Greetings_Users_To",
                table: "Greetings");

            migrationBuilder.DropIndex(
                name: "IX_Greetings_From",
                table: "Greetings");

            migrationBuilder.DropIndex(
                name: "IX_Greetings_To",
                table: "Greetings");

            migrationBuilder.AlterColumn<string>(
                name: "To",
                table: "Greetings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "From",
                table: "Greetings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
