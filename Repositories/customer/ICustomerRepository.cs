public interface ICustomerRepository
{
    Task<Customer?> getByIdAsync(int id);
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
}