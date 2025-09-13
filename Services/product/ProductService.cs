
using Microsoft.OpenApi.Models;

public class ProductService : IProductService
{
    private readonly IProductRepository _ProductRepo;
    public ProductService(IProductRepository productRepo)
    {
        _ProductRepo = productRepo;
    }

    public async Task<ProductResponseDTO> CreateAsync(CreateProductDto dto)
    {
        var newProduct = new Products
        {
            ProductName = dto.ProductName,
            Price = dto.Price,
            CategoryID = dto.CtegoryID,
            Stock = dto.Stock
        };

        var CreateProduct = await _ProductRepo.AddAsync(newProduct);
        var result = new ProductResponseDTO
        {
            ProductID = CreateProduct.ProductID,
            ProductName = CreateProduct.ProductName,
            Price = CreateProduct.Price,
            CategoryName = CreateProduct.Category?.CategoryName ?? ""

        };
        return result;
    }

    public async Task<IEnumerable<ProductResponseDTO>> GetAllProduct()
    {
        var products = await _ProductRepo.GetAllAsync();
        var result = products.Select(p => new ProductResponseDTO
        {
            ProductID = p.ProductID,
            ProductName = p.ProductName,
            Price = p.Price,
            Stock = p.Stock,
            CategoryName = p.Category?.CategoryName ?? ""

        });
        return result;

    }

    public async Task<ProductResponseDTO?> GetByIdAsync(int id)
    {
        var product = await _ProductRepo.GetProductAsync(id);
        if (product == null) throw new KeyNotFoundException("product not found");

        return new ProductResponseDTO
        {
            ProductID = product.ProductID,
            ProductName = product.ProductName,
            Price = product.Price,
            CategoryName = product.Category?.CategoryName ?? ""
        };
    }

    public async Task<PagedResponse<ProductResponseDTO>> GetProductsAsync(FilterDto filter)
    {
        var (products, totalRecords) = await _ProductRepo.GetFilteredproductSync(filter);

        var totalPages = (int)Math.Ceiling(totalRecords / (double)filter.PageSize);

        var data = products.Select(p => new ProductResponseDTO
        {
            ProductID = p.ProductID,
            ProductName = p.ProductName,
            Price = p.Price,
            Stock = p.Stock,
            CategoryName = p.Category != null ? p.Category.CategoryName : string.Empty
        });

        return new PagedResponse<ProductResponseDTO>
        {
            Success = true,
            Messages = "Success get products",
            Data = data,
            TotalRecords = totalRecords,
            CurrentPage = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalPages = totalPages,
            NextPage = filter.PageNumber < totalPages ? filter.PageNumber + 1 : null,
            PreviousPage = filter.PageNumber > 1 ? filter.PageNumber - 1 : null
        };
    }

    public async Task<Products?> UpdateAsync(int id, ProducUpdatetDto dto)
    {
        var product = await _ProductRepo.GetProductAsync(id);
        if (product == null) throw new KeyNotFoundException("product not found"); ;

        //semua field wajib di isi
        if (string.IsNullOrEmpty(dto.ProductName) ||
        !dto.Price.HasValue ||
        !dto.Stock.HasValue ||
        !dto.CategoryID.HasValue)
        {
            return null;

        }
        product.ProductName = dto.ProductName;
        product.Price = dto.Price.Value;
        product.Stock = dto.Stock.Value;
        product.CategoryID = dto.CategoryID.Value;

        await _ProductRepo.UpdateAsync(product);
        return product;


    }

    public async Task<Products?> PatchAsync(int id, ProducUpdatetDto dto)
    {
        var product = await _ProductRepo.GetProductAsync(id);
        if (product == null) return null;

        //update hanya field yang di isi
        if (!string.IsNullOrEmpty(dto.ProductName)) product.ProductName = dto.ProductName;
        if (dto.Price > 0) product.Price = dto.Price.Value;
        if (dto.Stock > 0) product.Stock = dto.Stock.Value;
        if (dto.CategoryID.HasValue) product.CategoryID = dto.CategoryID.Value;

        await _ProductRepo.UpdateAsync(product);
        return product;

    }
    //soft delete
    public async Task<bool> SoftDeleteAsync(int id)
    {
        var product = await _ProductRepo.GetProductAsync(id);
        if (product == null) return false;

        product.IsActive = false;
        await _ProductRepo.UpdateAsync(product);
        return true;
    }

    //hard delete
    public async Task<bool> HarddeleteAsync(int id)
    {
        var product = await _ProductRepo.GetProductAsync(id);
        if (product == null) return false;

        product.IsActive = false;
        await _ProductRepo.DeleteAsync(product);
        return true;

    }

}