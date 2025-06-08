using Dapper;
using StudentPortal.Data;
using StudentPortal.Models.DTOs.Auth;
using StudentPortal.Models.DTOs;
using System.Data;

public class AuthRepository : IAuthRepository
{
    private readonly IDbConnection _db;

    public AuthRepository(DapperContext context)
    {
        _db = context.CreateConnection();
    }

    public async Task<int> RegisterStudentAsync(RegisterDto dto)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Name", dto.Name);
        parameters.Add("@Email", dto.Email);
        parameters.Add("@PasswordHash", dto.Password); // Hashed already in service
        parameters.Add("@Batch", dto.Batch);
        parameters.Add("@Dob", dto.Dob);

        return await _db.ExecuteAsync("RegisterStudent", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> RegisterFacultyAsync(RegisterDto dto)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Name", dto.Name);
        parameters.Add("@Email", dto.Email);
        parameters.Add("@PasswordHash", dto.Password); // Hashed already in service

        return await _db.ExecuteAsync("RegisterFaculty", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<UserDto?> GetStudentByEmailAsync(string email)
    {
        var storedProcedure = "GetStudentByEmail";
        return await _db.QueryFirstOrDefaultAsync<UserDto>(
            storedProcedure,
            new { Email = email },
            commandType: System.Data.CommandType.StoredProcedure);
    }


    public async Task<UserDto?> GetFacultyByEmailAsync(string email)
    {
        var storedProcedure = "GetFacultyByEmail";
        return await _db.QueryFirstOrDefaultAsync<UserDto>(
            storedProcedure,
            new { Email = email },
            commandType: System.Data.CommandType.StoredProcedure);
    }


    public Task<UserDto?> LoginStudentAsync(LoginDto dto)
    {
        throw new NotImplementedException("Use GetStudentByEmailAsync and verify password in service.");
    }

    public Task<UserDto?> LoginFacultyAsync(LoginDto dto)
    {
        throw new NotImplementedException("Use GetFacultyByEmailAsync and verify password in service.");
    }
}
