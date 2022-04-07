using EduHomeFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.ViewModels
{
    #region AboutVM
    public class AboutVM
    {
        public About About { get; set; }
        public List<Teacher> Teachers { get; set; }
        public TeacherDetail TeacherDetail { get; set; }
        public List<TeacherSkill> TeacherSkills { get; set; }
        public List<TeacherSocialMedia> TeacherSocialMedias { get; set; }
        public TeacherPosition TeacherPosition { get; set; }
    }
    #endregion

}
