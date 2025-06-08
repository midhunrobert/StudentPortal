using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentPortal.Business.Interfaces;
using StudentPortal.Models.DTOs.Auth;
using StudentPortal.Models.DTOs.Student;

namespace StudentPortal.Web.Controllers
{
    public class StudentController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IStudentService _studentService;
        private readonly IWebHostEnvironment _env;

        public StudentController(IAuthService authService, IStudentService studentService, IWebHostEnvironment env)
        {
            _authService = authService;
            _studentService = studentService;
            _env = env;
        }

        [HttpGet]
        public IActionResult StudentLogin() => View(new LoginDto());

        [HttpPost]
public async Task<IActionResult> StudentLogin(LoginDto dto)
{
    var user = await _authService.LoginAsync(dto, "Student"); 
    if (user != null)
    {
        HttpContext.Session.SetString("UserId", user.UserId.ToString());
        HttpContext.Session.SetString("Role", "Student");

        var studentId = await _studentService.GetStudentIdByUserIdAsync(user.UserId);
        if (studentId != 0)
        {
            HttpContext.Session.SetString("StudentId", studentId.ToString());

            var student = await _studentService.GetStudentByIdAsync(studentId);
            if (student != null)
            {
                HttpContext.Session.SetString("StudentName", student.Name);
            }
        }

        return RedirectToAction("Dashboard", "Student");
    }

    ViewBag.Error = "Invalid credentials.";
    return View("StudentLogin", dto);
}



        [HttpGet]
        public IActionResult StudentRegister() => View(new RegisterDto());

        [HttpPost]
        public async Task<IActionResult> StudentRegister(RegisterDto dto)
        {
            dto.Role = "Student"; 
            bool result = await _authService.RegisterAsync(dto);
            if (!result)
            {
                ViewBag.Error = "Registration failed.";
                return View("StudentRegister", dto);
            }

            return RedirectToAction("StudentLogin", "Student");
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Student")
                return Unauthorized();

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ViewAssignments()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Student") return Unauthorized();

            var studentIdStr = HttpContext.Session.GetString("StudentId");
            if (string.IsNullOrEmpty(studentIdStr)) return RedirectToAction("StudentLogin");

            int studentId = int.Parse(studentIdStr);
            var assignments = await _studentService.GetAssignmentsByStudentIdAsync(studentId);

            return View(assignments);
        }
        [HttpGet]
        public IActionResult SubmitAssignment(int assignmentId)
        {
            ViewBag.AssignmentId = assignmentId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAssignment(int assignmentId, IFormFile file)
        {
            var studentIdStr = HttpContext.Session.GetString("StudentId");
            if (string.IsNullOrEmpty(studentIdStr)) return Unauthorized();
            int studentId = int.Parse(studentIdStr);

            if (file != null && file.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "submissions");
                Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                string dbFilePath = "/submissions/" + fileName;

                var existingSubmission = await _studentService.GetSubmissionByStudentAndAssignmentAsync(studentId, assignmentId);

                if (existingSubmission != null)
                {
                    string oldFilePath = Path.Combine(_env.WebRootPath, existingSubmission.FilePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }

                    existingSubmission.FilePath = dbFilePath;
                    existingSubmission.SubmissionDate = DateTime.Now;

                    await _studentService.UpdateSubmissionAsync(existingSubmission);

                    TempData["Success"] = "Assignment resubmitted successfully.";
                }
                else
                {
                    var dto = new SubmissionCreateDto
                    {
                        AssignmentId = assignmentId,
                        SubmittedByStudentId = studentId,
                        FilePath = dbFilePath
                    };

                    await _studentService.SubmitAssignmentAsync(dto);

                    TempData["Success"] = "Assignment submitted successfully.";
                }

                return RedirectToAction("ViewAssignments");
            }

            ViewBag.Error = "Please select a file.";
            ViewBag.AssignmentId = assignmentId;
            return View();
        }

        public async Task<IActionResult> Submissions()
        {
            int studentId = Convert.ToInt32(HttpContext.Session.GetString("StudentId"));
            var submissions = await _studentService.GetSubmissionsByStudentIdAsync(studentId);
            return View(submissions);
        }
        public async Task<IActionResult> ViewScores(int submissionId)
        {
            var scores = await _studentService.GetScoresBySubmissionIdAsync(submissionId);

            if (scores == null || !scores.Any())
            {
                return View("Error", "No scores found for this submission.");
            }

            ViewBag.TotalScored = scores.Sum(s => s.Score);
            ViewBag.TotalMax = scores.Sum(s => s.MaxScore);

            return View(scores);
        }
        public async Task<IActionResult> Performance()
        {
            int studentId = int.Parse(HttpContext.Session.GetString("StudentId"));
            var performance = await _studentService.GetStudentPerformanceAsync(studentId);
            return View(performance);
        }

        [HttpGet]
        public async Task<IActionResult> EditStudent(int id)
        {
            var student = await _studentService.GetStudentForEditAsync(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> EditStudent(StudentDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _studentService.UpdateStudentAsync(model);
            return RedirectToAction("Dashboard"); 
        }


    }
}
