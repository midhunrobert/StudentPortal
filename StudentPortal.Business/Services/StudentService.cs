using StudentPortal.Business.Interfaces;
using StudentPortal.Data.Interfaces;
using StudentPortal.Data.Repositories;
using StudentPortal.Models.DTOs.Faculty;
using StudentPortal.Models.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;

        public StudentService(IStudentRepository studentRepo)
        {
            _studentRepo = studentRepo;
        }

        public async Task<int> GetStudentIdByUserIdAsync(int userId)
        {
            return (await _studentRepo.GetStudentIdByUserIdAsync(userId)) ?? 0;
        }

        public async Task<IEnumerable<AssignmentDto>> GetAssignmentsByStudentIdAsync(int studentId)
        {
            var student = await _studentRepo.GetStudentByIdAsync(studentId);
            if (student == null) return Enumerable.Empty<AssignmentDto>();

            return await _studentRepo.GetAssignmentsByBatchAsync(student.Batch);
        }
        public async Task SubmitAssignmentAsync(SubmissionCreateDto dto)
        {
            await _studentRepo.SubmitAssignmentAsync(dto);
        }
        public async Task<IEnumerable<StudentSubmissionDto>> GetSubmissionsByStudentIdAsync(int studentId)
        {
            return await _studentRepo.GetSubmissionsByStudentIdAsync(studentId);
        }
        public async Task<IEnumerable<AssessmentScoreDto>> GetScoresBySubmissionIdAsync(int submissionId)
        {
            return await _studentRepo.GetScoresBySubmissionIdAsync(submissionId);
        }
        public async Task<IEnumerable<PerformanceDto>> GetStudentPerformanceAsync(int studentId)
        {
            return await _studentRepo.GetStudentPerformanceAsync(studentId);
        }
        public async Task<StudentDto> GetStudentForEditAsync(int studentId)
        {
            return await _studentRepo.GetStudentByIdAsync(studentId);
        }

        public async Task UpdateStudentAsync(StudentDto student)
        {
            await _studentRepo.UpdateStudentAsync(student);
        }

        public async Task<StudentDto> GetStudentByIdAsync(int studentId)
        {
            return await _studentRepo.GetStudentByIdAsync(studentId);
        }
        public async Task<StudentSubmissionDto?> GetSubmissionByStudentAndAssignmentAsync(int studentId, int assignmentId)
        {
            return await _studentRepo.GetSubmissionByStudentAndAssignmentAsync(studentId, assignmentId);
        }
        public async Task UpdateSubmissionAsync(StudentSubmissionDto dto)
        {
            await _studentRepo.UpdateSubmissionAsync(dto);
        }
    }

}
