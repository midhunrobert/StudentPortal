using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Models.DTOs.Faculty
{
    public class AssignmentUpdateDto
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; }
        public string Batch { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string FilePath { get; set; } 
    }

}
