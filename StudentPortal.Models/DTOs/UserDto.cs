namespace StudentPortal.Models.DTOs;

public class UserDto
{
    public int UserId { get; set; }
    public int RoleId { get; set; } 
    public string Email { get; set; }
    public string Name { get; set; }
    public string? Batch { get; set; } 

    public DateTime Dob { get; set; }


    public string PasswordHash { get; set; }
}
