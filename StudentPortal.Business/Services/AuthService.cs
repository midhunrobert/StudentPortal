using BCrypt.Net;
using StudentPortal.Models.DTOs.Auth;
using StudentPortal.Models.DTOs;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepo;

    public AuthService(IAuthRepository authRepo)
    {
        _authRepo = authRepo;
    }

    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        dto.Password = HashPassword(dto.Password);
        if (dto.Role == "Student")
            return await _authRepo.RegisterStudentAsync(dto) > 0;
        else if (dto.Role == "Faculty")
            return await _authRepo.RegisterFacultyAsync(dto) > 0;
        return false;
    }

    public async Task<UserDto?> LoginAsync(LoginDto dto, string role)
    {
        UserDto? user = role switch
        {
            "Student" => await _authRepo.GetStudentByEmailAsync(dto.Email),
            "Faculty" => await _authRepo.GetFacultyByEmailAsync(dto.Email),
            _ => null
        };

        if (user == null) return null;

        bool valid = VerifyPassword(dto.Password, user.PasswordHash);
        if (!valid) return null;

        return user;
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string plainPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
}
