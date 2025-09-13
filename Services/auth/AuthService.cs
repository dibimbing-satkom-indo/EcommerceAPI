using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher<User> _pwdHasher;

    public AuthService(
        IUnitOfWork uow,
        IPasswordHasher<User> passwordHasher)
    {
        _uow = uow;
        _pwdHasher = passwordHasher;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var exists = await _uow.Users.GetByUserNameAsync(dto.Username);
        if (exists != null)
            throw new ValidationException("Username sudah dipakai");

        var user = new User
        {
            UserName = dto.Username,
            Role = dto.Role,
            CreatedAt = DateTime.UtcNow,
            CustomerID = null
        };

        user.PasswordHash = _pwdHasher.HashPassword(user, dto.Password);

        await _uow.Users.AddAsync(user);
        await _uow.SaveChangesAsync();

        //var token = _jwt.GenerateToken(user);

        return new AuthResponseDto
        {
            UserID = user.UserID,
            Username = user.UserName,
            Role = user.Role,
            Token = "token",
            ProfileCompleted = false,
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _uow.Users.GetByUserNameAsync(dto.Username);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid Username or Password");

        var verify = _pwdHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (verify == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid Username or Password");
        }

        /*
      TODO: implement JWT soon
      var token = _jwt.gennerateToke(user)
      */
        return new AuthResponseDto
        {
            UserID = user.UserID,
            Username = user.UserName,
            Role = user.Role,
            Token = "token",
            ProfileCompleted = false
        };

    }

}