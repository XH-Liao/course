using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class TeacherClass
    {
        public Teacher teacher { get; set; }
        public List<Classes> classes { get; set; }
    }
}
