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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetAll()
    {
        var products = await _productService.GetAllProduct();
        if (products == null)
        {
            return NotFound(new BaseResponseDto<string>
            {
                Success = false,
                Messages = " product not found",
                Data = null
            });
        }
        return Ok(new BaseResponseDto<IEnumerable<ProductResponseDTO>>
        {
            Success = true,
            Messages = "success get all product",
            Data = products
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDTO>> GetProduct(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound(new BaseResponseDto<string>
            {
                Success = false,
                Messages = " product not found",
                Data = null
            });
        }

        return Ok(new BaseResponseDto<ProductResponseDTO>
        {
            Success = true,
            Messages = "success get product",
            Data = product
        });

    }

    [HttpGet("filter")]
    public async Task<ActionResult<PagedResponse<ProductResponseDTO>>> GetProducts([FromQuery] FilterDto filter)
    {
        var result = await _productService.GetProductsAsync(filter);
        return Ok(result);
    }


    [HttpPost]
    public async Task<ActionResult<ProductResponseDTO>> CreateProduct(CreateProductDto productDto)
    {
        var result = await _productService.CreateAsync(productDto);


        return StatusCode(201, new BaseResponseDto<ProductResponseDTO>
        {
            Success = true,
            Messages = "add product is success",
            Data = result

        });
    }



}