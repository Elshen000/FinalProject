using EduHomeFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomeFinal.ViewModels
{

    #region HomeVM
    public class HomeVM
    {
        public About About { get; set; }
        public List<Course> Course { get; set; }
        public List<Event> Event { get; set; }
        public List<Blog> Blog { get; set; }
        public List<Slider> Slider { get; set; }
        
        public Bio Bio { get; set; }
        public Subscribe Subscribe { get; set; }
    }
    #endregion

}
