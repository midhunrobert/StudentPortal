using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StudentPortal.Data.Interfaces;
using StudentPortal.Models.DTOs;
using StudentPortal.Models.DTOs.Faculty;
using StudentPortal.Models.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Data.Repositories
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly IDbConnection _db;

        public FacultyRepository(IConfiguration config)
        {
            _db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<UserDto>> GetAllStudentsAsync()
        {
            return await _db.QueryAsync<UserDto>(
                "GetAllStudentsF",
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task<int> CreateAssignmentAsync(AssignmentCreateDto dto, int facultyId, string filePath)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FacultyId", facultyId, DbType.Int32);
            parameters.Add("@Batch", dto.Batch, DbType.String);
            parameters.Add("@Title", dto.Title, DbType.String);
            parameters.Add("@Description", dto.Description, DbType.String);
            parameters.Add("@DueDate", dto.DueDate, DbType.DateTime);
            parameters.Add("@FilePath", filePath, DbType.String);

            var assignmentId = await _db.ExecuteScalarAsync<int>(
                "CreateAssignmentF",            // Stored Procedure name
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return assignmentId;
        }

        public async Task<int> GetFacultyIdByUserIdAsync(int userId)
        {
            var sql = "GetFacultyIdByUserIdF";
            return await _db.QuerySingleOrDefaultAsync<int>(
                sql,
                new { UserId = userId },
                commandType: System.Data.CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<string>> GetBatchesByFacultyAsync(int facultyId)
        {
            var parameters = new { FacultyId = facultyId };
            var batches = await _db.QueryAsync<string>(
                "GetBatchesByFaculty",
                parameters,
                commandType: CommandType.StoredProcedure);
            return batches;
        }
        public async Task<IEnumerable<AssignmentDto>> GetAssignmentsByBatchAsync(int facultyId, string batch)
        {
            var parameters = new { FacultyId = facultyId, Batch = batch };
            var result = await _db.QueryAsync<AssignmentDto>(
                "GetAssignmentsByBatch",
                parameters,
                commandType: CommandType.StoredProcedure);

            return result;
        }
        public async Task<int> CreateAssessmentAsync(AssessmentCreateDto dto, int facultyId)
        {
            var table = new DataTable();
            table.Columns.Add("CriterionName", typeof(string));
            table.Columns.Add("MaxScore", typeof(int));

            foreach (var c in dto.Criteria)
                table.Rows.Add(c.CriterionName, c.MaxScore);

            var parameters = new DynamicParameters();
            parameters.Add("@AssignmentId", dto.AssignmentId);
            parameters.Add("@FacultyId", facultyId);
            parameters.Add("@Criteria", table.AsTableValuedParameter("AssessmentCriteriaType"));

            var assessmentId = await _db.ExecuteScalarAsync<int>(
                "CreateAssessmentWithCriteria",
                parameters,
                commandType: CommandType.StoredProcedure);

            return assessmentId;
        }
        public async Task<IEnumerable<AssessmentSummaryDto>> GetAssessmentsByAssignmentIdAsync(int assignmentId)
        {
            var assessments = (await _db.QueryAsync<AssessmentSummaryDto>(
                "GetAssessmentsByAssignmentIdF",
                new { AssignmentId = assignmentId },
                commandType: System.Data.CommandType.StoredProcedure)).ToList();

            foreach (var assessment in assessments)
            {
                var criteria = await _db.QueryAsync<AssessmentCriteriaDto>(
                    "GetCriteriaByAssessmentIdF",
                    new { AssessmentId = assessment.AssessmentId },
                    commandType: System.Data.CommandType.StoredProcedure);

                assessment.Criteria = criteria.ToList();
            }

            return assessments;
        }


        public async Task<AssignmentDto> GetAssignmentDetailsAsync(int assignmentId)
        {
            var storedProcedure = "GetAssignmentDetailsF";
            return await _db.QueryFirstOrDefaultAsync<AssignmentDto>(
                storedProcedure,
                new { AssignmentId = assignmentId },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<SubmissionDto>> GetSubmissionsByAssignmentIdAsync(int assignmentId)
        {
            var storedProcedure = "GetSubmissionsByAssignmentIdF";
            return await _db.QueryAsync<SubmissionDto>(
                storedProcedure,
                new { AssignmentId = assignmentId },
                commandType: System.Data.CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<AssessmentCriteriaDto>> GetCriteriaByAssessmentIdAsync(int assessmentId)
        {
            var storedProcedure = "GetCriteriaByAssessmentIdF2";
            return await _db.QueryAsync<AssessmentCriteriaDto>(
                storedProcedure,
                new { AssessmentId = assessmentId },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> InsertAssessmentScoreAsync(AssessmentScoreDto score)
        {
            var storedProcedure = "InsertAssessmentScoreF";
            var parameters = new
            {
                score.SubmissionId,
                score.CriteriaId,
                score.Score,
                score.Comments,
                ScoredAt = DateTime.Now
            };

            return await _db.QuerySingleAsync<int>(
                storedProcedure,
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAssessmentScoreAsync(AssessmentScoreDto score)
        {
            var storedProcedure = "UpdateAssessmentScoreF";
            var parameters = new
            {
                score.SubmissionId,
                score.CriteriaId,
                score.Score,
                score.Comments,
                ScoredAt = DateTime.Now
            };

            return await _db.ExecuteAsync(
                storedProcedure,
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AssessmentScoreDto>> GetScoresBySubmissionIdAsync(int submissionId)
        {
            var storedProcedure = "GetScoresBySubmissionIdF";
            return await _db.QueryAsync<AssessmentScoreDto>(
                storedProcedure,
                new { SubmissionId = submissionId },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task SaveAssessmentScoreAsync(AssessmentScoreDto score)
        {
            var existingSql = @"SELECT COUNT(1) FROM AssessmentScores WHERE SubmissionId = @SubmissionId AND CriteriaId = @CriteriaId";
            var exists = await _db.ExecuteScalarAsync<int>(existingSql, new { score.SubmissionId, score.CriteriaId });

            if (exists > 0)
                await UpdateAssessmentScoreAsync(score);
            else
                await InsertAssessmentScoreAsync(score);

            var submissionSql = "SELECT AssignmentId FROM Submissions WHERE SubmissionId = @SubmissionId";
            var assignmentId = await _db.ExecuteScalarAsync<int>(submissionSql, new { score.SubmissionId });

            var assessmentSql = "SELECT TOP 1 AssessmentId FROM Assessments WHERE AssignmentId = @AssignmentId";
            var assessmentId = await _db.ExecuteScalarAsync<int>(assessmentSql, new { AssignmentId = assignmentId });

            var criteriaSql = "SELECT CriteriaId FROM AssessmentCriteria WHERE AssessmentId = @AssessmentId";
            var criteriaIds = (await _db.QueryAsync<int>(criteriaSql, new { AssessmentId = assessmentId })).ToList();

            var scoredSql = "SELECT CriteriaId FROM AssessmentScores WHERE SubmissionId = @SubmissionId";
            var scoredCriteriaIds = (await _db.QueryAsync<int>(scoredSql, new { score.SubmissionId })).ToList();

            var allScored = criteriaIds.All(id => scoredCriteriaIds.Contains(id));
            if (allScored)
            {
                var markScoredSql = "UPDATE Submissions SET IsScored = 1 WHERE SubmissionId = @SubmissionId";
                await _db.ExecuteAsync(markScoredSql, new { score.SubmissionId });
            }
        }


        public async Task<SubmissionDto> GetSubmissionByIdAsync(int submissionId)
        {
            var storedProcedure = "GetSubmissionByIdF";
            return await _db.QueryFirstOrDefaultAsync<SubmissionDto>(
                storedProcedure,
                new { SubmissionId = submissionId },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task MarkSubmissionAsScoredAsync(int submissionId)
        {
            var sql = "UPDATE Submissions SET IsScored = 1 WHERE SubmissionId = @SubmissionId";
            await _db.ExecuteAsync(sql, new { SubmissionId = submissionId });
        }

        public async Task<IEnumerable<StudentPerformanceDto>> GetStudentPerformanceByAssignmentAsync(int assignmentId)
        {
            var storedProcedure = "GetStudentPerformanceByAssignmentF";
            return await _db.QueryAsync<StudentPerformanceDto>(
                storedProcedure,
                new { AssignmentId = assignmentId },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<FacultyUpdateDto> GetFacultyByIdAsync(int id)
        {
            var storedProcedure = "GetFacultyByIdF";
            return await _db.QueryFirstOrDefaultAsync<FacultyUpdateDto>(
                storedProcedure,
                new { FacultyId = id },
                commandType: System.Data.CommandType.StoredProcedure);
        }


        public async Task<bool> UpdateFacultyAsync(FacultyUpdateDto dto)
        {
            var storedProcedure = "UpdateFacultyF";

            var result = await _db.ExecuteAsync(
                storedProcedure,
                new
                {
                    dto.FacultyId,
                    dto.Name,
                    dto.Email
                },
                commandType: System.Data.CommandType.StoredProcedure);

            return result > 0;
        }

        public async Task<Assessment> GetAssessmentWithCriteriaAsync(int assessmentId)
        {
            var sqlAssessment = "SELECT * FROM Assessments WHERE AssessmentId = @AssessmentId";
            var sqlCriteria = "SELECT * FROM AssessmentCriteria WHERE AssessmentId = @AssessmentId";

            var assessment = await _db.QueryFirstOrDefaultAsync<Assessment>(sqlAssessment, new { AssessmentId = assessmentId });
            if (assessment != null)
            {
                var criteria = await _db.QueryAsync<AssessmentCriteriaDto>(sqlCriteria, new { AssessmentId = assessmentId });
                assessment.AssessmentCriteria = criteria.ToList();
            }
            return assessment;
        }


        public async Task<bool> UpdateAssessmentAsync(AssessmentUpdateDto dto)
        {
            if (_db.State != ConnectionState.Open)
                _db.Open();

            using var transaction = _db.BeginTransaction();

            try
            {
                var updateAssessmentSql = @"
            UPDATE Assessments 
            SET AssignmentId = @AssignmentId 
            WHERE AssessmentId = @AssessmentId";

                await _db.ExecuteAsync(updateAssessmentSql, new
                {
                    dto.AssignmentId,
                    dto.AssessmentId
                }, transaction);

                var existingCriteriaSql = "SELECT CriteriaId FROM AssessmentCriteria WHERE AssessmentId = @AssessmentId";
                var existingCriteriaIds = (await _db.QueryAsync<int>(
                    existingCriteriaSql,
                    new { dto.AssessmentId },
                    transaction
                )).ToList();

                var dtoCriteria = dto.Criteria ?? new List<AssessmentCriteriaDto>();
                var dtoCriteriaIds = dtoCriteria.Where(c => c.CriteriaId != 0).Select(c => c.CriteriaId).ToList();

                var criteriaToDelete = existingCriteriaIds.Except(dtoCriteriaIds).ToList();
                if (criteriaToDelete.Any())
                {
                    var deleteScoresSql = "DELETE FROM AssessmentScores WHERE CriteriaId IN @Ids";
                    await _db.ExecuteAsync(deleteScoresSql, new { Ids = criteriaToDelete }, transaction);

                    var deleteCriteriaSql = "DELETE FROM AssessmentCriteria WHERE CriteriaId IN @Ids";
                    await _db.ExecuteAsync(deleteCriteriaSql, new { Ids = criteriaToDelete }, transaction);
                }

                var updateCriteriaSql = @"
            UPDATE AssessmentCriteria 
            SET CriterionName = @CriterionName, MaxScore = @MaxScore 
            WHERE CriteriaId = @CriteriaId";

                foreach (var crit in dtoCriteria.Where(c => c.CriteriaId != 0))
                {
                    await _db.ExecuteAsync(updateCriteriaSql, new
                    {
                        crit.CriterionName,
                        crit.MaxScore,
                        crit.CriteriaId
                    }, transaction);
                }

                var insertCriteriaSql = @"
            INSERT INTO AssessmentCriteria (AssessmentId, CriterionName, MaxScore) 
            VALUES (@AssessmentId, @CriterionName, @MaxScore)";

                foreach (var crit in dtoCriteria.Where(c => c.CriteriaId == 0))
                {
                    await _db.ExecuteAsync(insertCriteriaSql, new
                    {
                        AssessmentId = dto.AssessmentId,
                        crit.CriterionName,
                        crit.MaxScore
                    }, transaction);
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
        public async Task<AssignmentUpdateDto> GetAssignmentByIdAsync(int assignmentId)
        {
            var sql = @"SELECT AssignmentId, Title, Batch, Description, DueDate, FilePath
                FROM Assignments WHERE AssignmentId = @AssignmentId";

            return await _db.QueryFirstOrDefaultAsync<AssignmentUpdateDto>(sql, new { AssignmentId = assignmentId });
        }

        public async Task<bool> UpdateAssignmentAsync(AssignmentUpdateDto dto)
        {
            var storedProcedure = "UpdateAssignmentF";
            var affected = await _db.ExecuteAsync(
                storedProcedure,
                new
                {
                    dto.AssignmentId,
                    dto.Title,
                    dto.Batch,
                    dto.Description,
                    dto.DueDate,
                    dto.FilePath
                },
                commandType: System.Data.CommandType.StoredProcedure);

            return affected > 0;
        }








    }

}
