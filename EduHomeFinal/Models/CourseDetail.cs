using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Models
{
    public class CourseDetail
    {
        public int Id { get; set; }
        public string CourseAbout { get; set; }
        public string CourseApply { get; set; }
        public string CourseSertification { get; set; }
        public Course Course { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }

    }
}
