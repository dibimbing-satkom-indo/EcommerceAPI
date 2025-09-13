using Microsoft.EntityFrameworkCore;

public class UserReposioty : IUserRepository
{
    private readonly ApplicationDBContext _db;
    public UserReposioty(ApplicationDBContext db) => _db = db;

    public async Task AddAsync(User user)
            => await _db.Users.AddAsync(user);

    public Task UpdateAsync(User user)
    {
        _db.Users.Update(user);
        return Task.CompletedTask;

    }

    public async Task<User?> GetByIdAsync(int id)
            => await _db.Users.Include(u => u.Customer)
                .FirstOrDefaultAsync(u => u.UserID == id);


    public async Task<User?> GetByUserNameAsync(string userName)
        => await _db.Users.Include(u => u.Customer)
            .FirstOrDefaultAsync(u => u.UserName == userName);



}