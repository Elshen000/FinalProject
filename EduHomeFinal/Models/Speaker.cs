using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Models
{
    public class Speaker
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public string SpeakerName { get; set; }
        public bool IsDeactive { get; set; }
        public string SpeakerPlace { get; set; }
        public List<EventSpeaker> EventSpeaker { get; set; }
    }
}
