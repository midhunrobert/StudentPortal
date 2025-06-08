using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Models.DTOs.Faculty
{
    public class FacultyUpdateDto
    {
        public int FacultyId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; } 
    }


}
