using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StudentPortal.Business.Interfaces;
using StudentPortal.Data.Interfaces;
using StudentPortal.Data.Repositories;
using StudentPortal.Models.DTOs;
using StudentPortal.Models.DTOs.Faculty;
using StudentPortal.Models.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StudentPortal.Business.Services
{
    public class FacultyService : IFacultyService
    {
        private readonly IFacultyRepository _facultyRepo;

        public FacultyService(IFacultyRepository facultyRepo)
        {
            _facultyRepo = facultyRepo;
        }

        public async Task<IEnumerable<UserDto>> GetAllStudentsAsync()
        {
            return await _facultyRepo.GetAllStudentsAsync();
        }

        public async Task<int> CreateAssignmentAsync(AssignmentCreateDto dto, int facultyId, string filePath)
        {
            return await _facultyRepo.CreateAssignmentAsync(dto, facultyId, filePath);
        }
        public Task<int> GetFacultyIdByUserIdAsync(int userId)
        {
            return _facultyRepo.GetFacultyIdByUserIdAsync(userId);
        }
        public async Task<IEnumerable<string>> GetBatchesByFacultyAsync(int facultyId)
        {
            return await _facultyRepo.GetBatchesByFacultyAsync(facultyId);
        }
        public async Task<IEnumerable<AssignmentDto>> GetAssignmentsByBatchAsync(int facultyId, string batch)
        {
            return await _facultyRepo.GetAssignmentsByBatchAsync(facultyId, batch);
        }
        public async Task<int> CreateAssessmentAsync(AssessmentCreateDto dto, int facultyId)
        {
            return await _facultyRepo.CreateAssessmentAsync(dto, facultyId);
        }
        public async Task<IEnumerable<AssessmentSummaryDto>> GetAssessmentsByAssignmentIdAsync(int assignmentId)
        {
            return await _facultyRepo.GetAssessmentsByAssignmentIdAsync(assignmentId);
        }
        public async Task<AssignmentDto> GetAssignmentDetailsAsync(int assignmentId)
        {
            return await _facultyRepo.GetAssignmentDetailsAsync(assignmentId);
        }
        public async Task<IEnumerable<SubmissionDto>> GetSubmissionsByAssignmentIdAsync(int assignmentId)
        {
            return await _facultyRepo.GetSubmissionsByAssignmentIdAsync(assignmentId);
        }
        public async Task<IEnumerable<AssessmentCriteriaDto>> GetCriteriaByAssessmentIdAsync(int assessmentId)
        {
            return await _facultyRepo.GetCriteriaByAssessmentIdAsync(assessmentId);
        }

        public async Task SaveAssessmentScoreAsync(AssessmentScoreDto score)
        {

            await _facultyRepo.SaveAssessmentScoreAsync(score);
        }

        public async Task<IEnumerable<AssessmentScoreDto>> GetScoresBySubmissionIdAsync(int submissionId)
        {
            return await _facultyRepo.GetScoresBySubmissionIdAsync(submissionId);
        }
        public async Task<SubmissionDto> GetSubmissionByIdAsync(int submissionId)
        {
            return await _facultyRepo.GetSubmissionByIdAsync(submissionId);
        }
        public async Task<IEnumerable<StudentPerformanceDto>> GetStudentPerformanceByAssignmentAsync(int assignmentId)
        {
            return await _facultyRepo.GetStudentPerformanceByAssignmentAsync(assignmentId);
        }

        public async Task<FacultyUpdateDto> GetFacultyForEditAsync(int id)
    => await _facultyRepo.GetFacultyByIdAsync(id);

        public async Task<bool> UpdateFacultyAsync(FacultyUpdateDto dto)
            => await _facultyRepo.UpdateFacultyAsync(dto);

        public async Task<AssessmentUpdateDto> GetAssessmentForEditAsync(int assessmentId)
        {
            var assessment = await _facultyRepo.GetAssessmentWithCriteriaAsync(assessmentId);
            if (assessment == null) return null;

            return new AssessmentUpdateDto
            {
                AssessmentId = assessment.AssessmentId,
                AssignmentId = assessment.AssignmentId,
                Criteria = assessment.AssessmentCriteria.Select(c => new AssessmentCriteriaDto
                {
                    CriteriaId = c.CriteriaId,
                    CriterionName = c.CriterionName,
                    MaxScore = c.MaxScore
                }).ToList()
            };
        }

        public async Task<bool> UpdateAssessmentAsync(AssessmentUpdateDto dto)
        {
            return await _facultyRepo.UpdateAssessmentAsync(dto);
        }
        public async Task<AssignmentUpdateDto> GetAssignmentByIdAsync(int id)
        {
            return await _facultyRepo.GetAssignmentByIdAsync(id);
        }

        public async Task<bool> UpdateAssignmentAsync(AssignmentUpdateDto dto)
        {
            return await _facultyRepo.UpdateAssignmentAsync(dto);
        }




    }



}


