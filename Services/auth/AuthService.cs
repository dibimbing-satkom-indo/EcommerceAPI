using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher<User> _pwdHasher;
    private readonly IJwtService _jwt;

    public AuthService(
        IUnitOfWork uow,
        IPasswordHasher<User> passwordHasher,
        IJwtService jwt)
    {
        _uow = uow;
        _pwdHasher = passwordHasher;
        _jwt = jwt;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var exists = await _uow.Users.GetByUserNameAsync(dto.Username);
        if (exists != null)
            throw new ValidationException("Username sudah dipakai");

        var user = new User
        {
            UserName = dto.Username,
            Role = dto.Role.ToLower(),
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

        var token = _jwt.GenerateToke(user);
        return new AuthResponseDto
        {
            UserID = user.UserID,
            Username = user.UserName,
            Role = user.Role,
            Token = token,
            ProfileCompleted = false
        };

    }
    public async Task<AuthResponseDto> CompleteProfileAsyync(int id, CompleteProfileDto dto)
    {
        var user = await _uow.Users.GetByIdAsync(id);
        if (user == null) throw new KeyNotFoundException("User not found");

        Customer customer;

        if (user.CustomerID.HasValue)
        {
            // update existing customer
            customer = await _uow.Customers.getByIdAsync(user.CustomerID.Value)
                        ?? throw new KeyNotFoundException("User not found");
            customer.Name = dto.Name;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;
            customer.Address = dto.Address;
        }
        else
        {
            customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address
            };
        }
        await _uow.SaveChangesAsync();
        return new AuthResponseDto
        {
            UserID = user.UserID,
            Username = user.UserName,
            Role = user.Role,
            Token = "",
            ProfileCompleted = true,
            Customer = new CustomerProfileDto
            {
                CustomerID = customer.CustomerID,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address
            }
        };
    }

}