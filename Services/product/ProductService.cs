
public class ProductService : IProductService
{
    public async Task<IEnumerable<Products>> GetAllProduct()
    {
        await Task.Delay(100);
        return new List<Products>();
    }

    public async Task<Products?> GetPriductByID(int id)
    {
        await Task.Delay(100);
        return null;
    }
}