namespace StudentPortal.Models.DTOs.Auth;

public class RegisterDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Batch { get; set; } 
    public DateTime? Dob { get; set; } 
    public string Role { get; set; } 
}
