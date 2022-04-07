using Microsoft.EntityFrameworkCore.Migrations;

namespace EduHomeFinal.Migrations
{
    public partial class EditBioTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Vimeo",
                table: "Bios",
                newName: "Youtube");

            migrationBuilder.RenameColumn(
                name: "Twitter",
                table: "Bios",
                newName: "Linkedin");

            migrationBuilder.RenameColumn(
                name: "Pinterest",
                table: "Bios",
                newName: "Instagram");

            migrationBuilder.UpdateData(
                table: "Bios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Facebook", "Instagram", "Linkedin", "Logo", "Youtube" },
                values: new object[] { "https://www.facebook.com/itbrainacademy/", "https://www.instagram.com/proqramci_ol/", "https://www.linkedin.com/uas/login?session_redirect=https%3A%2F%2Fwww.linkedin.com%2Fcompany%2Fit-brains-training-center%2Fabout%2F", "footer-logo (1).png", "https://www.youtube.com/c/ITBrains" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Youtube",
                table: "Bios",
                newName: "Vimeo");

            migrationBuilder.RenameColumn(
                name: "Linkedin",
                table: "Bios",
                newName: "Twitter");

            migrationBuilder.RenameColumn(
                name: "Instagram",
                table: "Bios",
                newName: "Pinterest");

            migrationBuilder.UpdateData(
                table: "Bios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Facebook", "Logo", "Pinterest", "Twitter", "Vimeo" },
                values: new object[] { "https://www.facebook.com/", "logo2.png", "https://www.pinterest.com/", "https://twitter.com/", "https://vimeo.com/" });
        }
    }
}
