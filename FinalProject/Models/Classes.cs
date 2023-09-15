using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Classes
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="Class Name is required !")]
        [Display(Name ="Class Name")]
        public string ClassName { get; set; }

        [Required(ErrorMessage = "Course credit is required !")]
        [Display(Name ="Course Credit")]
        [Range(0, 10, ErrorMessage = "Please enter a number (0~10)")]
        public int CourseCredit { get; set; }

        [Required(ErrorMessage = "Teacher is required !")]
        public string Teacher { get; set; }

        [Required(ErrorMessage = "Classroom is required !")]
        [Display(Name ="Classroom")]
        public string Room { get; set; }

        [Required(ErrorMessage = "Day of Week is required !")]
        [Display(Name ="Day of Week")]
        public int DayOfWeek { get; set; }

        [Required(ErrorMessage = "Time is required !")]
        public int Time { get; set; }

        [StringLength(50,ErrorMessage ="Note must be less than 50")]
        public string Note { get; set; }
    }
}
