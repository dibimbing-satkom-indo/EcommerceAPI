using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;

    }

    // //recomeded approach - stringly typed
    // public async Task<ActionResult<Products>> GetProductByID(int id)
    // {
    //     await Task.Delay(100);
    //     var NewProduct = new Products
    //     {
    //         ProductName = "laptop",
    //         CategoryID = 1,
    //         Price = 1000,
    //         Stock = 5
    //     };
    //     if (NewProduct == null)
    //     {
    //         return NotFound(); //http 404

    //     }
    //     return NewProduct; // http 200 ok json data(implicit OK)

    // }


    // //when returning  deffrence type
    // public async Task<IActionResult> GetProductById(int id)
    // {
    //     await Task.Delay(100);
    //     var NewProduct = new Products
    //     {
    //         ProductName = "laptop",
    //         CategoryID = 1,
    //         Price = 1000,
    //         Stock = 5
    //     };
    //     if (NewProduct == null)
    //     {
    //         return NotFound(); //http 404

    //     }
    //     return Ok(NewProduct); // explicit OK

    // }



    [HttpGet]
    public async Task<ActionResult<IEnumerable<Products>>> GetProducts([FromQuery] FilterDto filter)
    {
        //url : GET /api/priducts?name=laptop&&minPrice=1000000&sortBy=price&pageNumber=2
        // otomatis di bind ke FilterDto object 

        await Task.Delay(100);
        //var product = "this is produuct stuf";
        return Ok(filter);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Products>> GetProduct(int id)
    {
        await Task.Delay(100);
        var result = $"get product by id = {id}";
        _logger.LogInformation("get data with {productID}: ", id); // information
        _logger.LogWarning("get data with {productID}: ", id); // warning
        _logger.LogError("get data with {productID}: ", id); // log error
        _logger.LogTrace("get data with {productID}: ", id); // tace
        _logger.LogDebug("get data with {productID}: ", id); // 
        return StatusCode(200, result);

    }

    [HttpGet("filter")] // query parameter GET api/product/search?name=laptop
    public async Task<ActionResult<Products>> SearchProduct(string cari)
    {
        await Task.Delay(100);
        var result = $"search product by name = {cari}";
        return StatusCode(200, result);

    }


    [HttpPost]
    public async Task<ActionResult<Products>> CreateProduct(CreateProductDto productDto)
    {
        await Task.Delay(100); //simulate call a servvice 

        var NewProduct = new Products
        {
            ProductName = productDto.ProductName,
            CategoryID = productDto.CtegoryID,
            Price = productDto.Price,
            Stock = productDto.Stock
        };

        return StatusCode(201, new BaseResponseDto<Products>
        {
            Success = true,
            Messages = "add product is success",
            Data = NewProduct

        });
    }



}