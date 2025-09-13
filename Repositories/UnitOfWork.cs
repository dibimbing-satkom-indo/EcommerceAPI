public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDBContext _db;
    public IUserRepository Users { get; set; }
    public ICustomerRepository Customers { get; set; }

    public UnitOfWork(
        ApplicationDBContext db,
        IUserRepository user,
        ICustomerRepository customer)
    {
        _db = db;
        Users = user;
        Customers = customer;

    }

    public async Task<int> SaveChangesAsync()
            => await _db.SaveChangesAsync();
}