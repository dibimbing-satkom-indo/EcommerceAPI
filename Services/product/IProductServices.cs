public interface IProductService
{
    Task<ProductResponseDTO> CreateAsync(CreateProductDto dto);
    Task<IEnumerable<ProductResponseDTO>> GetAllProduct();
    Task<ProductResponseDTO?> GetByIdAsync(int id);
    Task<PagedResponse<ProductResponseDTO>> GetProductsAsync(FilterDto filter);
}
