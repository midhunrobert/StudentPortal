using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Models.DTOs.Faculty
{
    public class AssessmentCriteriaDto
    {
        public int CriteriaId { get; set; }
        public int AssessmentId { get; set; }
        public string CriterionName { get; set; }
        public int MaxScore { get; set; }
    }

}
