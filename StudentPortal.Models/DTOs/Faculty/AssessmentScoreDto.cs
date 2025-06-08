using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Models.DTOs.Faculty
{
    public class AssessmentScoreDto
    {
        public int ScoreId { get; set; }
        public int SubmissionId { get; set; }
        public int CriteriaId { get; set; }
        public int Score { get; set; }
        public string Comments { get; set; }
        public DateTime? ScoredAt { get; set; }
        public string CriterionName { get; set; }  
        public int MaxScore { get; set; }         
    }

}
