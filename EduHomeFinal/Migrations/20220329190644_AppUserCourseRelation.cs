using Microsoft.EntityFrameworkCore.Migrations;

namespace EduHomeFinal.Migrations
{
    public partial class AppUserCourseRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Abouts",
                columns: new[] { "Id", "Description", "Icon", "Image", "Information", "Title" },
                values: new object[] { 1, "IT Brains Academy-nin ali məqsədi gənclərə və həvəsi olan hər kəsə IT və Proqramlaşdırma sahəsində karyera qurmaq və iş imkanları əldə etmək üçün dəstək olmaqdır. Operativ işləyən komandaya sahib olan  IT Brains Academy bugünə qədər 1000-lərlə tələbəyə həm əyani, həm də onlayn formada öz tövhəsini vermişdir. Tədris sonunda tələbələr müəyyən imtahanlar verərək sertifikatla təltif edilirlər. Eləcə də, biz sizi Microsoft, Cisco, CompTIA və.s. kimi nəhəng şirkətlərin imtahanlarına hazırlaşdıraraq sizin beynəlxalq sertifikatlı mütəxəssis olmağınıza zəmanət veririk.", "footer-logo.png", "ItBrains-160737_5d6f.jpeg", " IT Brains Academy həmçinin böyük məqalə və videodərs bazasıdır. Belə ki, burada öyrənmək istədiyiniz sahələrə uyğun siz öz müəllim heyətimizin hazırladığı materiallar və videodərslərlə təmin edilirsiniz. Tədris və sistemimizin keyfiyyətini artırmaq məqsədilə IT Brains Academy daim sizin təklif və iradlarınıza məmnuniyyətlə cavab verməyə hazırdır. ", "IT BRAINS-Ə XOŞ GƏLMİŞSİNİZ!" });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_AppUserId",
                table: "Courses",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_AppUserId",
                table: "Courses",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_AppUserId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_AppUserId",
                table: "Courses");

            migrationBuilder.DeleteData(
                table: "Abouts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Courses");
        }
    }
}
