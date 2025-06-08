using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Models.DTOs.Student
{
    public class StudentSubmissionDto
    {
        public int SubmissionId { get; set; }
        public int AssignmentId { get; set; }
        public string AssignmentTitle { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string FilePath { get; set; }
        public int IsScored { get; set; }
    }

}
