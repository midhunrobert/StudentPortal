using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Models.DTOs.Faculty
{
    public class SubmissionScoreDto
    {
        public int SubmissionId { get; set; }
        public int CriteriaId { get; set; }
        public string CriteriaName { get; set; }
        public int Score { get; set; }
        public string Comments { get; set; }
    }

}
