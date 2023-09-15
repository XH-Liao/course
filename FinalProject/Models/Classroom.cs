using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Classroom
    {
        [Key]
        [Required(ErrorMessage = "ID is required")]
        public int ID { get; set; }

        [Required(ErrorMessage ="Name is required")]
        [StringLength(20, ErrorMessage = "Length max is 20 !")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Use is required")]
        [Display(Name="Use")]
        [StringLength(20,ErrorMessage ="Length max is 20 !")]
        public string ToUse { get; set; }

        [Required(ErrorMessage = "Seat is required")]
        [Range(0,500,ErrorMessage ="It must be 0~500 !")]
        public int Seat { get; set; }

        [StringLength(45, ErrorMessage = "Length max is 45!")]
        public string Note { get; set; }
    }
}
