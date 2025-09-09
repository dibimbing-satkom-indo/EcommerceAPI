public interface IProductService
{

    Task<IEnumerable<Products>> GetAllProduct();
    Task<Products?> GetPriductByID(int id);
}
