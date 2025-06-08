using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using StudentPortal.Business.Interfaces;
using StudentPortal.Models.DTOs.Auth;
using StudentPortal.Models.DTOs.Faculty;
using System.ComponentModel;
using System.Drawing;

namespace StudentPortal.Controllers
{
    public class FacultyController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IFacultyService _facultyService;

        public FacultyController(IAuthService authService, IFacultyService facultyService)
        {
            _authService = authService;
            _facultyService = facultyService;
        }


        public IActionResult Dashboard()
        {
            var facultyIdStr = HttpContext.Session.GetString("FacultyId");
            if (string.IsNullOrEmpty(facultyIdStr))
            {
                return RedirectToAction("Login", "Auth");
            }

            int facultyId = int.Parse(facultyIdStr);
            ViewBag.FacultyId = facultyId;

            return View();
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult FacultyLogin()
        {
            return View(new LoginDto());
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult FacultyRegister()
        {
            return View(new RegisterDto { Role = "Faculty" });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> FacultyRegister(RegisterDto dto)
        {
            dto.Role = "Faculty"; 
            bool result = await _authService.RegisterAsync(dto);

            if (!result)
            {
                ViewBag.Error = "Registration failed.";
                return View("FacultyRegister", dto);
            }

            return RedirectToAction("FacultyLogin");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> FacultyLogin(LoginDto dto)
        {
            var user = await _authService.LoginAsync(dto, "Faculty");

            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("Role", "Faculty");

                var facultyId = await _facultyService.GetFacultyIdByUserIdAsync(user.UserId);
                HttpContext.Session.SetString("FacultyId", facultyId.ToString());

                return RedirectToAction("Dashboard", "Faculty");
            }

            ViewBag.Error = "Invalid credentials.";
            return View(dto);
        }
        public async Task<IActionResult> Students()
        {
            var students = await _facultyService.GetAllStudentsAsync();
            return View(students);
        }
        [HttpGet]
        public IActionResult CreateAssignment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssignment(AssignmentCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var facultyId = int.Parse(HttpContext.Session.GetString("FacultyId"));

            string filePath = null;
            if (dto.File != null)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploads); 

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.File.FileName);
                var path = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await dto.File.CopyToAsync(stream);
                }

                filePath = "/uploads/" + fileName;
            }

            await _facultyService.CreateAssignmentAsync(dto, facultyId, filePath);

            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        public async Task<IActionResult> Batches()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Faculty") return Unauthorized();

            var userId = int.Parse(HttpContext.Session.GetString("UserId"));
            var facultyId = await _facultyService.GetFacultyIdByUserIdAsync(userId); 

            var batches = await _facultyService.GetBatchesByFacultyAsync(facultyId);
            return View(batches);
        }
        public async Task<IActionResult> AssignmentsByBatch(string batch)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Faculty") return Unauthorized();

            var userId = int.Parse(HttpContext.Session.GetString("UserId"));
            var facultyId = await _facultyService.GetFacultyIdByUserIdAsync(userId);

            var assignments = await _facultyService.GetAssignmentsByBatchAsync(facultyId, batch);
            ViewBag.Batch = batch;

            return View(assignments);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAssessment(int assignmentId)
        {
            ViewBag.AssignmentId = assignmentId;
            return View(new AssessmentCreateDto { AssignmentId = assignmentId, Criteria = new List<AssessmentCriteriaDto>() });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssessment(AssessmentCreateDto dto)
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));
            var facultyId = await _facultyService.GetFacultyIdByUserIdAsync(userId);

            int newId = await _facultyService.CreateAssessmentAsync(dto, facultyId);

            return RedirectToAction("Dashboard"); 
        }
        public async Task<IActionResult> ViewAssessments(int assignmentId)
        {
            var assessments = await _facultyService.GetAssessmentsByAssignmentIdAsync(assignmentId);
            var assignment = await _facultyService.GetAssignmentDetailsAsync(assignmentId);

            ViewBag.AssignmentTitle = assignment.Title;
            return View(assessments); 
        }
        public async Task<IActionResult> ViewSubmissions(int assignmentId)
        {
            var submissions = await _facultyService.GetSubmissionsByAssignmentIdAsync(assignmentId);
            ViewBag.AssignmentId = assignmentId;
            return View(submissions);
        }

        [HttpGet]
        public async Task<IActionResult> ScoreSubmission(int submissionId)
        {
            var scores = await _facultyService.GetScoresBySubmissionIdAsync(submissionId);
            if (scores != null && scores.Any())
            {
                return View("ViewScores", scores);
            }

            var submission = await _facultyService.GetSubmissionByIdAsync(submissionId);
            if (submission == null)
            {
                return NotFound();
            }

            var assignment = await _facultyService.GetAssignmentDetailsAsync(submission.AssignmentId);
            if (assignment == null)
            {
                return NotFound();
            }

            var assessments = await _facultyService.GetAssessmentsByAssignmentIdAsync(assignment.AssignmentId);
            if (assessments == null || !assessments.Any())
            {
                return NotFound("No assessments created for this assignment.");
            }

            var criteria = await _facultyService.GetCriteriaByAssessmentIdAsync(assessments.First().AssessmentId);
            if (criteria == null || !criteria.Any())
            {
                return NotFound("No assessment criteria found.");
            }

            var dtoList = criteria.Select(c => new AssessmentScoreDto
            {
                SubmissionId = submissionId,
                CriteriaId = c.CriteriaId,
                Score = 0,
                Comments = ""
            }).ToList();

            return View(dtoList); 
        }

        [HttpPost]
        public async Task<IActionResult> ScoreSubmission(List<AssessmentScoreDto> scores, int AssignmentId)
        {
            foreach (var score in scores)
            {
                await _facultyService.SaveAssessmentScoreAsync(score);
            }

            return RedirectToAction("ViewSubmissions", new { assignmentId = AssignmentId });
        }
        public async Task<IActionResult> PerformanceChart(int assignmentId)
        {
            var data = await _facultyService.GetStudentPerformanceByAssignmentAsync(assignmentId);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile(int id)
        {
            var faculty = await _facultyService.GetFacultyForEditAsync(id);
            if (faculty == null) return NotFound();
            return View(faculty);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(FacultyUpdateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var updated = await _facultyService.UpdateFacultyAsync(dto);
            if (!updated) return NotFound();

            TempData["Success"] = "Faculty details updated successfully.";
            return RedirectToAction("Dashboard");
        }


        [HttpGet]
        public async Task<IActionResult> EditAssessment(int id)
        {
            var dto = await _facultyService.GetAssessmentForEditAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> EditAssessment(AssessmentUpdateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var updated = await _facultyService.UpdateAssessmentAsync(dto);
            if (!updated) return NotFound();

            TempData["Success"] = "Assessment updated successfully.";
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> EditAssignment(int id)
        {
            var assignment = await _facultyService.GetAssignmentByIdAsync(id);
            if (assignment == null)
                return NotFound();

            return View(assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAssignment(int id, AssignmentUpdateDto dto)
        {
            if (id != dto.AssignmentId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(dto);

            var success = await _facultyService.UpdateAssignmentAsync(dto);
            if (success)
                return RedirectToAction("Dashboard"); 

            ModelState.AddModelError("", "Update failed.");
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadPerformanceExcel(int assignmentId)
        {
            

            var data = await _facultyService.GetStudentPerformanceByAssignmentAsync(assignmentId);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Performance");

                worksheet.Cells[1, 1].Value = "Student Name";
                worksheet.Cells[1, 2].Value = "Total Score";
                worksheet.Cells[1, 3].Value = "Max Score";

                // Apply bold font to headers
                using (var range = worksheet.Cells[1, 1, 1, 3])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                // Data
                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1].Value = item.StudentName;
                    worksheet.Cells[row, 2].Value = item.TotalScore;
                    worksheet.Cells[row, 3].Value = item.MaxScore;
                    row++;
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                var fileName = $"Performance_Assignment_{assignmentId}.xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(stream, contentType, fileName);
            }
        }




    }
}
