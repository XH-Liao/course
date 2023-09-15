using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Teacher
    {
        [Key]
        [Required(ErrorMessage ="ID is required")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(30,ErrorMessage ="Max length is 30 !")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Office is required")]
        [StringLength(30, ErrorMessage = "Max length is 30 !")]
        public string Office { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(45, ErrorMessage = "Max length is 45 !")]
        public string Note { get; set; }
    }
}
