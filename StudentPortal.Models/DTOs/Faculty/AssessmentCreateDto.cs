using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Models.DTOs.Faculty
{
    public class AssessmentCreateDto
    {
        public int AssignmentId { get; set; }
        public List<AssessmentCriteriaDto> Criteria { get; set; }
    }

}
