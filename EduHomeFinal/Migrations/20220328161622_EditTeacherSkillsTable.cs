using Microsoft.EntityFrameworkCore.Migrations;

namespace EduHomeFinal.Migrations
{
    public partial class EditTeacherSkillsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SkillName",
                table: "TeacherSkills",
                newName: "SkillName6");

            migrationBuilder.RenameColumn(
                name: "SkillLevel",
                table: "TeacherSkills",
                newName: "SkillLevel6");

            migrationBuilder.AddColumn<int>(
                name: "SkillLevel1",
                table: "TeacherSkills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SkillLevel2",
                table: "TeacherSkills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SkillLevel3",
                table: "TeacherSkills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SkillLevel4",
                table: "TeacherSkills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SkillLevel5",
                table: "TeacherSkills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SkillName1",
                table: "TeacherSkills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkillName2",
                table: "TeacherSkills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkillName3",
                table: "TeacherSkills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkillName4",
                table: "TeacherSkills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkillName5",
                table: "TeacherSkills",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkillLevel1",
                table: "TeacherSkills");

            migrationBuilder.DropColumn(
                name: "SkillLevel2",
                table: "TeacherSkills");

            migrationBuilder.DropColumn(
                name: "SkillLevel3",
                table: "TeacherSkills");

            migrationBuilder.DropColumn(
                name: "SkillLevel4",
                table: "TeacherSkills");

            migrationBuilder.DropColumn(
                name: "SkillLevel5",
                table: "TeacherSkills");

            migrationBuilder.DropColumn(
                name: "SkillName1",
                table: "TeacherSkills");

            migrationBuilder.DropColumn(
                name: "SkillName2",
                table: "TeacherSkills");

            migrationBuilder.DropColumn(
                name: "SkillName3",
                table: "TeacherSkills");

            migrationBuilder.DropColumn(
                name: "SkillName4",
                table: "TeacherSkills");

            migrationBuilder.DropColumn(
                name: "SkillName5",
                table: "TeacherSkills");

            migrationBuilder.RenameColumn(
                name: "SkillName6",
                table: "TeacherSkills",
                newName: "SkillName");

            migrationBuilder.RenameColumn(
                name: "SkillLevel6",
                table: "TeacherSkills",
                newName: "SkillLevel");
        }
    }
}
