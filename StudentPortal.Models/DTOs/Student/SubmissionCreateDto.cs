using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Models.DTOs.Student
{
    public class SubmissionCreateDto
    {
        public int AssignmentId { get; set; }
        public int SubmittedByStudentId { get; set; }
        public string FilePath { get; set; }
    }
}
