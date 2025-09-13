using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDBContext _dbCtx;
    public ProductRepository(ApplicationDBContext dbCtx)
    {
        _dbCtx = dbCtx;

    }

    public async Task<Products> AddAsync(Products product)
    {
        _dbCtx.Products.Add(product);
        await _dbCtx.SaveChangesAsync();
        return product;
    }

    public async Task<IEnumerable<Products>> GetAllAsync()
    {
        var products = await _dbCtx.Products
        .Where(p => p.IsActive == true)
        .Include(p => p.Category)
        .ToListAsync();

        return products;
    }

    public async Task<Products?> GetProductAsync(int id)
    {
        var product = await _dbCtx.Products
        .Include(p => p.Category)
        .FirstOrDefaultAsync(p => p.ProductID == id);

        return product;
    }

    public async Task<(IEnumerable<Products>, int)> GetFilteredproductSync(FilterDto filter)
    {
        var query = _dbCtx.Products
        .Include(p => p.Category)
        .AsQueryable();

        // search by ProductName
        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(p => p.ProductName.Contains(filter.Name));

        //filter by category name
        if (!string.IsNullOrEmpty(filter.CategoryName))
            query = query.Where(p => p.Category != null &&
            p.Category.CategoryName.Contains(filter.CategoryName));

        //filter by price
        if (filter.MinPrice.HasValue)
            query = query.Where(p => p.Price >= filter.MinPrice.Value);

        if (filter.MinPrice.HasValue)
            query = query.Where(p => p.Price <= filter.MinPrice.Value);

        //filer by stock
        if (filter.InStock.HasValue)
            query = filter.InStock.Value
            ? query.Where(p => p.Stock > 0)
            : query.Where(p => p.Stock <= 0);

        //sorting
        query = filter.SortBy?.ToLower() switch
        {
            "price" => filter.Sortorder == "desc"
            ? query.OrderByDescending(p => p.Price)
            : query.OrderBy(p => p.Price),

            _ => filter.Sortorder == "desc"
                ? query.OrderByDescending(p => p.ProductName)
                : query.OrderBy(p => p.ProductName)
        };

        //total data
        var totalData = await query.CountAsync();

        //pagination
        var products = await query
        .Skip((filter.PageNumber - 1) * filter.PageSize)
        .Take(filter.PageSize)
        .ToListAsync();

        return (products, totalData);
    }

    public async Task UpdateAsync(Products product)
    {
        _dbCtx.Products.Update(product);
        await _dbCtx.SaveChangesAsync();

    }

    //hard delete
    public async Task DeleteAsync(Products products)
    {
        _dbCtx.Products.Remove(products);
        await _dbCtx.SaveChangesAsync();
    }
}