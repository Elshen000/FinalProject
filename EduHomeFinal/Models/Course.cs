using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public string CourseName { get; set; }
        public bool IsDeactive { get; set; }
        public string CourseDescription { get; set; }
        public CourseDetail CourseDetail { get; set; }
        public List<CourseCategory> CourseCategories { get; set; }
        public CourseFeature CourseFeatures { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }

       

    }
}
