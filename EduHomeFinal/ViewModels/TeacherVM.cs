using EduHomeFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.ViewModels
{
    #region MyRegion
    public class TeacherVM
    {
        public TeacherSkill TeacherSkills { get; set; }
        public List<Teacher> Teachers { get; set; }
        public Teacher Teacher { get; set; }
        public About About { get; set; }

        public TeacherDetail TeacherDetail { get; set; }
        public List<TeacherSocialMedia> TeacherSocialMedias { get; set; }
        public TeacherPosition TeacherPosition { get; set; }
        public TeacherSocialMedia TeacherSocialMedia { get; set; }
        public CourseDetail CourseDetail { get; set; }
        public List<Course> Courses { get; set; }

    }
    #endregion

}
