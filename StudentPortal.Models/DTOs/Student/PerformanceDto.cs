using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Models.DTOs.Student
{
    public class PerformanceDto
    {
        public int AssignmentId { get; set; }
        public string AssignmentTitle { get; set; }
        public string SubmissionDate { get; set; } 
        public int TotalScore { get; set; }
        public int MaxScore { get; set; }
        public decimal? Percentage { get; set; }
        public DateTime DueDate { get; set; } 
    }


}
