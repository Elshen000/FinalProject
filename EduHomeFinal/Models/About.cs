using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Models
{
    public class About
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public string Icon { get; set; }
        [NotMapped]
        public IFormFile Photo2 { get; set; }
        public string Title { get; set; }
        public string Information { get; set; }
        public string Description { get; set; }
    }
}
