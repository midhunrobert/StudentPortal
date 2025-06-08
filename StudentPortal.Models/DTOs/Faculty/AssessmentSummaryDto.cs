using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Models.DTOs.Faculty
{
    public class AssessmentSummaryDto
    {
        public int AssessmentId { get; set; }
        public string AssignmentTitle { get; set; }
        public string Batch { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CriteriaCount { get; set; }
        public List<AssessmentCriteriaDto> Criteria { get; set; }

    }
}
