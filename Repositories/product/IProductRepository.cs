public interface IProductRepository
{
    Task<Products> AddAsync(Products product);
    Task<IEnumerable<Products>> GetAllAsync();
    Task<Products?> GetProductAsync(int id);
    Task<(IEnumerable<Products>, int)> GetFilteredproductSync(FilterDto filter);
}