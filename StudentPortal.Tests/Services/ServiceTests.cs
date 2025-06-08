using Moq;
using StudentPortal.Business.Services;
using StudentPortal.Data.Interfaces;
using StudentPortal.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Tests.Services
{
    public class ServiceTests
    {
        private readonly Mock<IFacultyRepository> _mockRepo;
        private readonly FacultyService _facultyService;

        public ServiceTests()
        {
            _mockRepo = new Mock<IFacultyRepository>();
            _facultyService = new FacultyService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetFacultyIdByUserIdAsync_ReturnsFacultyId()
        {
            int userId = 42;
            int expectedFacultyId = 101;

            _mockRepo.Setup(repo => repo.GetFacultyIdByUserIdAsync(userId))
                     .ReturnsAsync(expectedFacultyId);

            var result = await _facultyService.GetFacultyIdByUserIdAsync(userId);

            Assert.Equal(expectedFacultyId, result);
        }

        public async Task GetAllStudentsAsync_ReturnsListOfStudents()
        {
            var expectedStudents = new List<UserDto>
        {
            new UserDto { Name = "Alice", Email = "alice@example.com" },
            new UserDto { Name = "Bob", Email = "bob@example.com" }
        };

            _mockRepo.Setup(repo => repo.GetAllStudentsAsync())
                     .ReturnsAsync(expectedStudents);

            var result = await _facultyService.GetAllStudentsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, ((List<UserDto>)result).Count);
            Assert.Equal("Alice", ((List<UserDto>)result)[0].Name);
        }

    }
}
