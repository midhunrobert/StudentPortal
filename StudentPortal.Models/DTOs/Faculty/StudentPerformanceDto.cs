using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Models.DTOs.Faculty
{
    public class StudentPerformanceDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public double TotalScore { get; set; }
        public double MaxScore { get; set; }
    }

}
