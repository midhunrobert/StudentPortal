using StudentPortal.Models.DTOs.Auth;
using StudentPortal.Models.DTOs;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto dto);
    Task<UserDto?> LoginAsync(LoginDto dto, string role);
}
