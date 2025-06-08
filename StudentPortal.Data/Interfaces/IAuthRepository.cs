using StudentPortal.Models.DTOs;
using StudentPortal.Models.DTOs.Auth;

public interface IAuthRepository
{
    Task<int> RegisterStudentAsync(RegisterDto dto);
    Task<int> RegisterFacultyAsync(RegisterDto dto);

    Task<UserDto?> LoginStudentAsync(LoginDto dto);
    Task<UserDto?> LoginFacultyAsync(LoginDto dto);

    Task<UserDto?> GetStudentByEmailAsync(string email);
    Task<UserDto?> GetFacultyByEmailAsync(string email);
}
