using StudentPortal.Models.DTOs;
using StudentPortal.Models.DTOs.Faculty;
using StudentPortal.Models.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Business.Interfaces
{
    public interface IFacultyService
    {
        Task<IEnumerable<UserDto>> GetAllStudentsAsync();
        Task<int> CreateAssignmentAsync(AssignmentCreateDto dto, int facultyId, string filePath);
        Task<int> GetFacultyIdByUserIdAsync(int userId);
        Task<IEnumerable<string>> GetBatchesByFacultyAsync(int facultyId);
        Task<IEnumerable<AssignmentDto>> GetAssignmentsByBatchAsync(int facultyId, string batch);
        Task<int> CreateAssessmentAsync(AssessmentCreateDto dto, int facultyId);
        Task<IEnumerable<AssessmentSummaryDto>> GetAssessmentsByAssignmentIdAsync(int assignmentId);
        Task<AssignmentDto> GetAssignmentDetailsAsync(int assignmentId);
        Task<IEnumerable<SubmissionDto>> GetSubmissionsByAssignmentIdAsync(int assignmentId);
        Task<IEnumerable<AssessmentCriteriaDto>> GetCriteriaByAssessmentIdAsync(int assessmentId);
        Task SaveAssessmentScoreAsync(AssessmentScoreDto score);
        Task<IEnumerable<AssessmentScoreDto>> GetScoresBySubmissionIdAsync(int submissionId);
        Task<SubmissionDto> GetSubmissionByIdAsync(int submissionId);
        Task<IEnumerable<StudentPerformanceDto>> GetStudentPerformanceByAssignmentAsync(int assignmentId);
        Task<FacultyUpdateDto> GetFacultyForEditAsync(int id);
        Task<bool> UpdateFacultyAsync(FacultyUpdateDto dto);
        Task<AssessmentUpdateDto> GetAssessmentForEditAsync(int assessmentId);
        Task<bool> UpdateAssessmentAsync(AssessmentUpdateDto dto);
        Task<AssignmentUpdateDto> GetAssignmentByIdAsync(int id);
        Task<bool> UpdateAssignmentAsync(AssignmentUpdateDto dto);







    }

}
