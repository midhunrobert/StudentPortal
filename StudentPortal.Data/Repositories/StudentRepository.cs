using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StudentPortal.Data.Interfaces;
using StudentPortal.Models.DTOs.Faculty;
using StudentPortal.Models.DTOs.Student;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace StudentPortal.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IDbConnection _db;

        public StudentRepository(IConfiguration config)
        {
            _db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task<int?> GetStudentIdByUserIdAsync(int userId)
        {
            return await _db.QuerySingleOrDefaultAsync<int?>(
                "GetStudentIdByUserId",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<AssignmentDto>> GetAssignmentsByBatchAsync(string batch)
        {
            return await _db.QueryAsync<AssignmentDto>(
                "GetAssignmentsByBatchS",
                new { Batch = batch },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<StudentDto?> GetStudentByIdAsync(int studentId)
        {
            return await _db.QuerySingleOrDefaultAsync<StudentDto>(
                "GetStudentByIdS",
                new { StudentId = studentId },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task SubmitAssignmentAsync(SubmissionCreateDto dto)
        {
            await _db.ExecuteAsync(
                "SubmitAssignmentS",
                new
                {
                    AssignmentId = dto.AssignmentId,
                    SubmittedByStudentId = dto.SubmittedByStudentId,
                    FilePath = dto.FilePath
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<IEnumerable<StudentSubmissionDto>> GetSubmissionsByStudentIdAsync(int studentId)
        {
            return await _db.QueryAsync<StudentSubmissionDto>(
                "GetSubmissionsByStudentIdS",
                new { StudentId = studentId },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<IEnumerable<AssessmentScoreDto>> GetScoresBySubmissionIdAsync(int submissionId)
        {
            return await _db.QueryAsync<AssessmentScoreDto>(
                "GetScoresBySubmissionIdS",
                new { SubmissionId = submissionId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<PerformanceDto>> GetStudentPerformanceAsync(int studentId)
        {
            return await _db.QueryAsync<PerformanceDto>(
                "GetStudentPerformanceByStudentIdS",
                new { StudentId = studentId },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task UpdateStudentAsync(StudentDto student)
        {
            await _db.ExecuteAsync(
                "UpdateStudent",
                student,
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<StudentSubmissionDto?> GetSubmissionByStudentAndAssignmentAsync(int studentId, int assignmentId)
        {
            return await _db.QueryFirstOrDefaultAsync<StudentSubmissionDto>(
                "GetSubmissionByStudentAndAssignmentS",
                new { AssignmentId = assignmentId, StudentId = studentId },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task UpdateSubmissionAsync(StudentSubmissionDto dto)
        {
            await _db.ExecuteAsync(
                "UpdateSubmissionS",
                new
                {
                    dto.SubmissionId,
                    dto.FilePath,
                    dto.SubmissionDate
                },
                commandType: CommandType.StoredProcedure
            );
        }




    }
}
