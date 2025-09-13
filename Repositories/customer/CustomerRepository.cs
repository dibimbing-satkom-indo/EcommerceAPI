using Microsoft.EntityFrameworkCore;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDBContext _db;
    public CustomerRepository(ApplicationDBContext db) => _db = db;

    public async Task<Customer?> getByIdAsync(int id)
            => await _db.Customers.FirstOrDefaultAsync(c => c.CustomerID == id);

    public async Task AddAsync(Customer customer)
            => await _db.Customers.AddAsync(customer);

    public Task UpdateAsync(Customer customer)
    {
        _db.Customers.Update(customer);
        return Task.CompletedTask;
    }

}