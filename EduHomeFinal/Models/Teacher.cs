using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public bool IsDeactive { get; set; }
        public string HeaderName { get; set; }
        public string HeaderImage { get; set; }
        [NotMapped]
        public IFormFile Photo2 { get; set; }
        
        public TeacherDetail TeacherDetail { get; set; }
        public TeacherSkill TeacherSkill { get; set; }
        public TeacherPosition TeacherPosition { get; set; }
      
        public TeacherSocialMedia TeacherSocialMedia { get; set; }
    }
}
