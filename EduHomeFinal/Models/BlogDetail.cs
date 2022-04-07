using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Models
{
    public class BlogDetail
    {
        public int Id { get; set; }
        public string About { get; set; }
        public string SubTitle { get; set; }
        public string Title { get; set; }
        public string BlogDesciription { get; set; }
        public DateTime CreateTime { get; set; }
        public string Title2 { get; set; }
        public Blog Blog { get; set; }
        [ForeignKey("Blog")]
        public int BlogId { get; set; }


    }
}
