using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Models.DTOs.Faculty
{
    public class Assessment
    {
        public int AssessmentId { get; set; }
        public int AssignmentId { get; set; }

        public DateTime CreatedAt { get; set; }
        public List<AssessmentCriteriaDto> AssessmentCriteria { get; set; } = new List<AssessmentCriteriaDto>();
    }

}
