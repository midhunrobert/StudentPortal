using StudentPortal.Models.DTOs.Faculty;
using StudentPortal.Models.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Data.Interfaces
{
    public interface IStudentRepository
    {
        Task<int?> GetStudentIdByUserIdAsync(int userId);
        Task<IEnumerable<AssignmentDto>> GetAssignmentsByBatchAsync(string batch);
        Task<StudentDto?> GetStudentByIdAsync(int studentId);
        Task SubmitAssignmentAsync(SubmissionCreateDto dto);
        Task<IEnumerable<StudentSubmissionDto>> GetSubmissionsByStudentIdAsync(int studentId);
        Task<IEnumerable<AssessmentScoreDto>> GetScoresBySubmissionIdAsync(int submissionId);
        Task<IEnumerable<PerformanceDto>> GetStudentPerformanceAsync(int studentId);
        Task UpdateStudentAsync(StudentDto student);
        Task<StudentSubmissionDto?> GetSubmissionByStudentAndAssignmentAsync(int studentId, int assignmentId);
        Task UpdateSubmissionAsync(StudentSubmissionDto dto);




    }
}
