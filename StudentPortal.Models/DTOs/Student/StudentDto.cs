using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Models.DTOs.Student
{
    public class StudentDto
    {
        public int StudentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Batch { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
    }
}
