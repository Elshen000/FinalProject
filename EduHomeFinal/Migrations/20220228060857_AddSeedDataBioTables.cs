using Microsoft.EntityFrameworkCore.Migrations;

namespace EduHomeFinal.Migrations
{
    public partial class AddSeedDataBioTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bios",
                columns: new[] { "Id", "Facebook", "Logo", "Pinterest", "Twitter", "Vimeo" },
                values: new object[] { 1, "https://www.facebook.com/", "logo2.png", "https://www.pinterest.com/", "https://twitter.com/", "https://vimeo.com/" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bios",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
