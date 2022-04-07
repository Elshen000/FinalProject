using EduHomeFinal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {


        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {

        }

        public DbSet<Bio> Bios { get; set; }
       
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherDetail> TeacherDetails { get; set; }
        public DbSet<TeacherSkill> TeacherSkills { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogDetail> BlogDetails { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<CourseDetail> CourseDetails { get; set; }
        public DbSet<CourseFeature> CourseFeatures { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventDetail> EventDetails { get; set; }
        public DbSet<EventSpeaker> EventSpeakers { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<TeacherPosition> TeacherPositions { get; set; }
        public DbSet<TeacherSocialMedia> TeacherSocialMedias { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<NoticeBoard> NoticeBoards { get; set; }
        public DbSet<ReplyMessage> ReplyMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Bio>().HasData(
                new Bio {Id=1,Logo= "footer-logo (1).png", Facebook= "https://www.facebook.com/itbrainacademy/", Instagram= "https://www.instagram.com/proqramci_ol/", Linkedin= "https://www.linkedin.com/uas/login?session_redirect=https%3A%2F%2Fwww.linkedin.com%2Fcompany%2Fit-brains-training-center%2Fabout%2F", Youtube= "https://www.youtube.com/c/ITBrains" }
                );

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<About>().HasData(
                new About { Id=1,Image= "ItBrains-160737_5d6f.jpeg",Title= "IT BRAINS-Ə XOŞ GƏLMİŞSİNİZ!", Description= "IT Brains Academy-nin ali məqsədi gənclərə və həvəsi olan hər kəsə IT və Proqramlaşdırma sahəsində karyera qurmaq və iş imkanları əldə etmək üçün dəstək olmaqdır. Operativ işləyən komandaya sahib olan  IT Brains Academy bugünə qədər 1000-lərlə tələbəyə həm əyani, həm də onlayn formada öz tövhəsini vermişdir. Tədris sonunda tələbələr müəyyən imtahanlar verərək sertifikatla təltif edilirlər. Eləcə də, biz sizi Microsoft, Cisco, CompTIA və.s. kimi nəhəng şirkətlərin imtahanlarına hazırlaşdıraraq sizin beynəlxalq sertifikatlı mütəxəssis olmağınıza zəmanət veririk.", Information= " IT Brains Academy həmçinin böyük məqalə və videodərs bazasıdır. Belə ki, burada öyrənmək istədiyiniz sahələrə uyğun siz öz müəllim heyətimizin hazırladığı materiallar və videodərslərlə təmin edilirsiniz. Tədris və sistemimizin keyfiyyətini artırmaq məqsədilə IT Brains Academy daim sizin təklif və iradlarınıza məmnuniyyətlə cavab verməyə hazırdır. ", Icon= "footer-logo.png", }
                );
        }
       
    }
}
