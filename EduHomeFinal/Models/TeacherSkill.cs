using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Models
{
    public class TeacherSkill
    {
        public int Id { get; set; }
      
        public string SkillName1 { get; set; }
        public int SkillLevel1 { get; set; }
        public string SkillName2 { get; set; }
        public int SkillLevel2 { get; set; }
        public string SkillName3 { get; set; }
        public int SkillLevel3 { get; set; }
        public string SkillName4 { get; set; }
        public int SkillLevel4 { get; set; }
        public string SkillName5 { get; set; }
        public int SkillLevel5 { get; set; }
        public string SkillName6 { get; set; }
        public int SkillLevel6 { get; set; }
        public Teacher Teacher { get; set; }
        [ForeignKey("Teacher")]
        public int TeacherId { get; set; }
    }
}
