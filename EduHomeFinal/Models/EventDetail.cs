using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.Models
{
    public class EventDetail
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string SubTitle2 { get; set; }
        public Event Event { get; set; }
        [ForeignKey("Event")]
        public int EventId { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string EventPlace { get; set; }


    }
}
